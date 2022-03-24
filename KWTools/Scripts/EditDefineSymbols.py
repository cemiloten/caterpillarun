#!/usr/bin/python
#
#  Downloads localisations from Google Docs to local JSON.
#
#  Copyright (c) 2011 Kwalee Ltd. All rights reserved.
#

import os
import re
import argparse

PROJECT_SETTINGS_PATH = "../../ProjectSettings/ProjectSettings.asset"
ALL_SYMBOLS_PATTERN = r"scriptingDefineSymbols:\n((?:[^\d]*(?:\d: .*))*)"
ALL_PLATFORM_SYMBOLS_PATTERN = r"[^\d]*(\d): (.*)"


if __name__ == "__main__":
    arg_parser = argparse.ArgumentParser(description='Add or remove define symbols')
    arg_parser.add_argument('-a', '--add', help='Symbols to add separated by space.', required=False, nargs='*')
    arg_parser.add_argument('-r', '--remove', help='Symbols to remove separated by space.', required=False, nargs='*')
    args = arg_parser.parse_args()

    symbols_to_add = args.add if args.add is not None else []
    symbols_to_remove = args.remove if args.remove is not None else []

    if os.path.exists(PROJECT_SETTINGS_PATH):
        with open(PROJECT_SETTINGS_PATH, mode='r') as reader:
            project_settings = reader.read()

        all_symbols = str(re.search(ALL_SYMBOLS_PATTERN, project_settings).group())
        all_platform_symbols = re.findall(ALL_PLATFORM_SYMBOLS_PATTERN, all_symbols)
        for platform_symbols in all_platform_symbols:
            symbols = platform_symbols[1].split(';')
            new_symbols = list(set().union(symbols, symbols_to_add))
            new_symbols = [symbol for symbol in new_symbols if symbol not in symbols_to_remove]
            all_symbols = all_symbols.replace(platform_symbols[0] + ': ' + platform_symbols[1], platform_symbols[0] + ': ' + ';'.join(new_symbols))
        project_settings = re.sub(ALL_SYMBOLS_PATTERN, all_symbols, project_settings)

        with open(PROJECT_SETTINGS_PATH, mode='w') as writer:
            writer.write(project_settings)

        print('Project settings were edited successfully')
    else:
        print('Could not find project settings at ' + PROJECT_SETTINGS_PATH)
