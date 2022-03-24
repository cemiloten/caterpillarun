using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Reflection;

public class KwaleeSDKImporter : MonoBehaviour
{
    private const string PYTHON_PATH = "virtual_env/kwalee/bin/python";
    private const string SDK_SETUP_PATH = "Assets/Kwalee/Editor/SDKSetup";
    private const string PYTHON_SETUP_FILENAME = "SetupPythonEnv";
    private const string DEPLOY_SCRIPT_FILENAME = "deploySDK.py";

    private static readonly string PYTHON_FULL_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), PYTHON_PATH);
    private static readonly string SDK_SETUP_FULL_PATH = Application.dataPath + "/../" + SDK_SETUP_PATH;

    [MenuItem("Kwalee/Import Kwalee SDK", priority = 10100)]
    private static void FindAndImportKwaleeSDK()
    {
        string sdkPath = EditorUtility.OpenFilePanel("Kwalee SDK zip file", "", "zip");

        if (sdkPath != "")
        {
            ImportKwaleeSDK(sdkPath);
        }
    }

    [MenuItem("Kwalee/Install Python", priority = 10100)]
    private static void InstallVirtualPython()
    {
        if (!File.Exists(PYTHON_FULL_PATH))
        {
            RunBashScript(PYTHON_SETUP_FILENAME, SDK_SETUP_FULL_PATH);
        }
    }

    public static void ImportKwaleeSDK(string sdkPath)
    {
        if (File.Exists(sdkPath))
        {
            string deployScriptPath = SDK_SETUP_PATH + "/" + DEPLOY_SCRIPT_FILENAME;
            if (File.Exists(deployScriptPath))
            {
                string deployCommand = "{0} -z \"{1}\" -u True";
                RunPythonScript(String.Format(deployCommand, deployScriptPath, sdkPath), "", "Kwalee SDK imported successfully", ImportFinished);
            }
            else
            {
                Debug.LogErrorFormat("Could not find deploy script at '{0}'.", deployScriptPath);
            }
        }
        else
        {
            Debug.LogErrorFormat("File '{0}' not found.", sdkPath);
        }
    }

    private static void ImportFinished()
    {
        AssetDatabase.Refresh();
        InvokeStaticMethod("UpdateConfigurationMenuItem", "UpdateConfigurationFiles");
        InvokeStaticMethod("SwitchSDKMenuItems", "LiveDLLs");
    }

    private static void InvokeStaticMethod(string className, string methodName)
    {
        Type classType = Type.GetType(className);
        if (classType == null)
        {
            Assembly KWCoreEditor = Array.Find(AppDomain.CurrentDomain.GetAssemblies(), a => a.GetName().Name.Equals("KWCoreEditor"));
            if (KWCoreEditor != null)
            {
                classType = KWCoreEditor.GetType(className);
            }
        }

        if (classType != null)
        {
            classType.InvokeMember(methodName, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null);
        }
    }

    private static void RunPythonScript(string arguments, string workingDirectory, string successMessage, System.Action callback = null)
    {
        InstallVirtualPython();

        workingDirectory = Path.GetFullPath(Path.Combine(Application.dataPath, "../")) + workingDirectory;

        System.Diagnostics.Process pythonProcess = new System.Diagnostics.Process();
        pythonProcess.StartInfo.FileName = PYTHON_FULL_PATH;
        pythonProcess.StartInfo.Arguments = arguments;
        pythonProcess.StartInfo.RedirectStandardError = true;
        pythonProcess.StartInfo.RedirectStandardOutput = true;
        pythonProcess.StartInfo.CreateNoWindow = true;
        pythonProcess.StartInfo.WorkingDirectory = workingDirectory;
        pythonProcess.StartInfo.UseShellExecute = false;
        pythonProcess.Start();

        string errorOutput = pythonProcess.StandardError.ReadToEnd();

        if (errorOutput != "")
        {
            Debug.LogError("<color=red>ERROR</color>\n" + errorOutput);
        }
        else
        {
            Debug.Log("<color=green>" + successMessage + "</color>\n");
        }

        pythonProcess.WaitForExit();
        pythonProcess.Close();

        if (callback != null)
        {
            callback();
        }
    }

    private static void RunBashScript(string filename, string workingDirectory)
    {
        if (!Directory.Exists(workingDirectory))
        {
            UnityEngine.Debug.LogErrorFormat("Could not find path: '{0}'.", workingDirectory);
            return;
        }

        System.Diagnostics.Process scriptProcess = new System.Diagnostics.Process();
        scriptProcess.StartInfo.FileName = "bash";
        scriptProcess.StartInfo.Arguments = filename;
        scriptProcess.StartInfo.RedirectStandardError = true;
        scriptProcess.StartInfo.RedirectStandardOutput = true;
        scriptProcess.StartInfo.CreateNoWindow = false;
        scriptProcess.StartInfo.WorkingDirectory = workingDirectory;
        scriptProcess.StartInfo.UseShellExecute = false;
        scriptProcess.Start();

        string errorOutput = scriptProcess.StandardError.ReadToEnd();
        if (errorOutput != "")
        {
            UnityEngine.Debug.LogError("<color=red>ERROR</color> " + errorOutput);
        }

        scriptProcess.WaitForExit();
        scriptProcess.Close();
    }
}
