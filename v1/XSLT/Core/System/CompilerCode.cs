using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Dom
{
    public class CompilerCode
    {
        public static Type[] Run(string cs_src) {

            CSharpCodeProvider provider = new CSharpCodeProvider();

            CompilerParameters p = new CompilerParameters();
            p.GenerateExecutable = false;
            p.GenerateInMemory = true;
            p.GenerateExecutable = false;
            //parameters.OutputAssembly = outputAssemblyFile;

            // Set the level at which the compiler 
            // should start displaying warnings.
            p.WarningLevel = 3;

            // Set whether to treat all warnings as errors.
            p.TreatWarningsAsErrors = false;

            // Set compiler argument to optimize output.
            p.CompilerOptions = "/optimize";

            // Add an assembly reference.
            p.ReferencedAssemblies.Add("System.dll");

            //if (provider.Supports(GeneratorSupport.EntryPointMethod))
            //{
            //    // Specify the class that contains the main method of the executable.
            //    p.MainClass = "model.oItem";
            //}


            CompilerResults result = provider.CompileAssemblyFromSource(p, cs_src);

            if (result.Errors.Count > 0)
            {
                Console.WriteLine("Compile ERROR");
            }
            else
            {
                //Console.WriteLine("Compile OK");
                //Console.WriteLine("Assembly Path:" + result.PathToAssembly);
                //Console.WriteLine("Assembly Name:" + result.CompiledAssembly.FullName);

                return result.CompiledAssembly.GetTypes();
            }
            return new Type[] { };
        }


        public static bool CompileCode(CodeDomProvider provider,
    String sourceFile,
    String exeFile)
        {

            CompilerParameters cp = new CompilerParameters();

            // Generate an executable instead of 
            // a class library.
            cp.GenerateExecutable = true;

            // Set the assembly file name to generate.
            cp.OutputAssembly = exeFile;

            // Generate debug information.
            cp.IncludeDebugInformation = true;

            // Add an assembly reference.
            cp.ReferencedAssemblies.Add("System.dll");

            // Save the assembly as a physical file.
            cp.GenerateInMemory = false;

            // Set the level at which the compiler 
            // should start displaying warnings.
            cp.WarningLevel = 3;

            // Set whether to treat all warnings as errors.
            cp.TreatWarningsAsErrors = false;

            // Set compiler argument to optimize output.
            cp.CompilerOptions = "/optimize";

            // Set a temporary files collection.
            // The TempFileCollection stores the temporary files
            // generated during a build in the current directory,
            // and does not delete them after compilation.
            cp.TempFiles = new TempFileCollection(".", true);

            if (provider.Supports(GeneratorSupport.EntryPointMethod))
            {
                // Specify the class that contains 
                // the main method of the executable.
                cp.MainClass = "Samples.Class1";
            }

            if (Directory.Exists("Resources"))
            {
                if (provider.Supports(GeneratorSupport.Resources))
                {
                    // Set the embedded resource file of the assembly.
                    // This is useful for culture-neutral resources,
                    // or default (fallback) resources.
                    cp.EmbeddedResources.Add("Resources\\Default.resources");

                    // Set the linked resource reference files of the assembly.
                    // These resources are included in separate assembly files,
                    // typically localized for a specific language and culture.
                    cp.LinkedResources.Add("Resources\\nb-no.resources");
                }
            }

            // Invoke compilation.
            CompilerResults cr = provider.CompileAssemblyFromFile(cp, sourceFile);

            if (cr.Errors.Count > 0)
            {
                // Display compilation errors.
                Console.WriteLine("Errors building {0} into {1}",
                    sourceFile, cr.PathToAssembly);
                foreach (CompilerError ce in cr.Errors)
                {
                    Console.WriteLine("  {0}", ce.ToString());
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Source {0} built into {1} successfully.",
                    sourceFile, cr.PathToAssembly);
                Console.WriteLine("{0} temporary files created during the compilation.",
                    cp.TempFiles.Count.ToString());

            }

            // Return the results of compilation.
            if (cr.Errors.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
