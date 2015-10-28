// --
// -- TestCop http://testcop.codeplex.com
// -- License http://testcop.codeplex.com/license
// -- Copyright 2014
// --
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.Tree;
using TestCop.Plugin.Highlighting;

[assembly: RegisterConfigurableSeverity(
       TestClassNameSuffixWarning.SeverityId,
       null, Highlighter.HighlightingGroup,
       "All test classes should have the same suffix",
       "TestCop : To easily identify a test class by its name it must have the configured suffix",
       Severity.ERROR,
       false)]

namespace TestCop.Plugin.Highlighting
{
    [ConfigurableSeverityHighlighting(SeverityId, CSharpLanguage.Name)]
    public class TestClassNameSuffixWarning : AbstractTestClassNameWarning
    {
        internal const string SeverityId = "TestClassNameSuffixWarning";

        public TestClassNameSuffixWarning(string expectedSuffix, IAccessRightsOwnerDeclaration declaration)
            : base(string.Format("Test class names should end with '{0}'.", expectedSuffix), declaration)
        {
        }

        public override bool IsValid()
        {
            if (HighlightingSettingsManager.Instance.GetConfigurableSeverity(SeverityId, base.Declaration.GetSolution())
                == Severity.DO_NOT_SHOW) return false;

            return true;
        }
    }
}