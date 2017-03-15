using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CSharp;
using UnityEngine;

public class MessageTranslator : MonoBehaviour 
{

	#region Fields

    public List<Translation> translations = new List<Translation>()
    {
        new Translation()
    };

    private static CSharpCodeProvider provider;
    private static CompilerParameters compilerParams;

    private static Dictionary<Translation, Type> compiledTranslations = new Dictionary<Translation, Type>();

	#endregion
	
	
	#region Constants

    //class Name, from, to
    private const string SourceTemplate =
@"using System;
using UnityEngine;

public class {0}_{1} : MonoBehaviour
{{
    public void {0}()
    {{
        SendMessage(""{1}"", SendMessageOptions.DontRequireReceiver);
    }}
    public void {0}(object obj)
    {{
        SendMessage(""{1}"", obj, SendMessageOptions.DontRequireReceiver);
    }}
}}";


	
	#endregion
	

	#region Behaviours

	private void Awake()
	{   
        var uncompiled = translations.Where(t => !compiledTranslations.ContainsKey(t));
        if (uncompiled.Any())
        {
            BuildCompiler();
            var sources = uncompiled.Select(t => String.Format(SourceTemplate, t.From, t.To)).ToArray();
            
            CompilerResults results = provider.CompileAssemblyFromSource(compilerParams, sources);

            foreach (var t in uncompiled)
            {
                compiledTranslations.Add(t, results.CompiledAssembly.GetType(t.From + "_" + t.To));
            }
        }

        foreach (Translation t in translations)
        {
            gameObject.AddComponent(compiledTranslations[t]);
        }

        Destroy(this);
	}
	#endregion
		
	
	#region Private Methods

    private static void BuildCompiler()
    {
        Dictionary<string, string> providerOptions = new Dictionary<string, string>
        {
            {"CompilerVersion", "v2.0"}
        };
        provider = new CSharpCodeProvider(providerOptions);       

        compilerParams = new CompilerParameters
        {
            CompilerOptions = "/target:library /optimize /warn:0",
            GenerateInMemory = true,
            GenerateExecutable = false
        };

        compilerParams.ReferencedAssemblies.AddRange(new[] 
        {
            Assembly.GetAssembly(typeof(MonoBehaviour)).Location,
            "System.dll",
            "System.core.dll",
        });
    }

	
	#endregion
	
}

[Serializable]
public class Translation
{
    public string From = "From";
    public string To = "To";

    public override bool Equals(object obj)
    {
        if (obj is Translation)
        {
            Translation other = (Translation)obj;
            return (From == other.From && To == other.To);
        }
        else
        {
            return false;
        }
    }

    public override int GetHashCode()
    {
        return (From + To).GetHashCode();
    }
}