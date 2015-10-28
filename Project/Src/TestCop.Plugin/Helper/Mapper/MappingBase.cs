using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Util;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.UI.Avalon.TreeListView;
using JetBrains.Util;
using TestCop.Plugin.Extensions;

namespace TestCop.Plugin.Helper.Mapper
{
    public abstract class MappingBase : IProjectMappingHeper
    {
        public abstract IList<TestCopProjectItem> GetAssociatedProject(IProject currentProject, IProjectFile projectFile, string currentNameSpace);

        public virtual bool IsTestProject(IProject project)
        {
            string currentProjectNamespace = project.GetDefaultNamespace();
            if (string.IsNullOrEmpty(currentProjectNamespace)) return false;

            return TestingRegEx.IsMatch(currentProjectNamespace);
        }

        protected virtual Regex TestingRegEx
        {
            get
            {
                var testNameSpacePattern = Settings.TestProjectToCodeProjectNameSpaceRegEx;
                var regEx = new Regex(testNameSpacePattern);
                return regEx;
            }
        }

        public void DumpDebug(ISolution solution)
        {
            var rx = TestingRegEx;
            solution.GetAllCodeProjects().ForEach(
                p => ResharperHelper.AppendLineToOutputWindow("\tProject Namespace:" + p.GetDefaultNamespace()
                                                              +
                                                              (rx.IsMatch(p.GetDefaultNamespace() ?? "")
                                                                  ? " matches "
                                                                  : " does not match ")
                                                              + rx));
        }

        protected static TestFileAnalysisSettings Settings
        {
            get { return TestCopSettingsManager.Instance.Settings; }
        }

        public static bool RegexReplace(string regexPattern, string regexReplaceText, string inputString, out string resultString)
        {
            resultString = "";
            var regex = new Regex(regexPattern);
            var match = regex.Match(inputString);

            if (match.Success && match.Groups.Count > 1)
            {
                if (regexReplaceText.IsNullOrEmpty() || regexReplaceText == "*")
                {
                    for (int i = 1; i < match.Groups.Count; i++) resultString += match.Groups[i].Value;
                    return true;
                }

                resultString = regex.Replace(inputString, regexReplaceText);
                return true;
            }
            return false;
        }

        protected static IEnumerable<String> AssociatedFileNames(TestFileAnalysisSettings settings, IProjectFile projectFile)
        {
            return AssociatedFileNames(settings,projectFile.Location.NameWithoutExtension);
        }

        private static IEnumerable<String> AssociatedFileNames(TestFileAnalysisSettings settings, string baseFileNameWithoutExt)
        {        
            string classNameFromFileName = baseFileNameWithoutExt;

            foreach (var suffix in settings.TestClassSuffixes())
            {
                if (baseFileNameWithoutExt.EndsWith(suffix))
                {
                    classNameFromFileName = baseFileNameWithoutExt.Split(new[] { '.' }, 2)[0].RemoveTrailing(suffix);
                    break;
                }
            }

            if (baseFileNameWithoutExt != classNameFromFileName)
            {
                yield return classNameFromFileName;
            }
            else
            {
                foreach (var suffix in settings.TestClassSuffixes())
                {
                    yield return string.Format(@"{0}{1}", classNameFromFileName, suffix);//e.g. Class1Tests
                    yield return string.Format(@"{0}\..*{1}", classNameFromFileName, suffix);  //e.g. Class1.SecurityTests                  
                }
            }
        }
    }
}