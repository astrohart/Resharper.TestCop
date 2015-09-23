﻿// // --
// // -- TestCop http://testcop.codeplex.com
// // -- License http://testcop.codeplex.com/license
// // -- Copyright 2015
// // --

using JetBrains.Application.Settings;
using NUnit.Framework;

namespace TestCop.Plugin.Tests.RenameRefactoring
{
    [TestFixture]
    public class RenameTestFilesTooRefactoringSingleTestProjectPerSolutionTests : RenameTestFilesTooRefactoringTestBase
    {        
        protected override string RelativeTestDataPath
        {
            get { return @"SingleTestProjectForManyCodeProject\ClassToTestNavigation"; }
        }
        
        protected override string SolutionName
        {
            get { return @"TestApplication3.sln"; }
        }

        protected override void ConfigureForTestCopStrategy(IContextBoundSettingsStore settingsStore)
        {
            SingleTestProjectToMultipleCodeProject.ClassToTestFileNavigationTests.SetupTestCopSettings(settingsStore);
        }

        [Test]
        public void RenameClassRenamesTestFilesTooTest()
        {            
            DoRenameTest(
                  @"<MyCorp.TestApplication3.DAL>\NS1\NS2\ClassA.cs"
                , @"<MyCorp.TestApplication3.Tests>\DAL\NS1\NS2\ClassATests.cs->NewClassTests");
        }    
    }
}