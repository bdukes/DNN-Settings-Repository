Imports NUnit.Framework
Imports DotNetNuke.Collections
Imports DotNetNuke.UI.Modules
Imports DotNetNuke.Entities.Controllers
Imports NSubstitute

<TestFixture>
Public Class SettingsRepositoryTests
    <Test>
    Public Sub GetValue_GivenCustomConverter_ConvertsSettingValue()
        StubHostSettings(New Dictionary(Of String, String) From {{"MyModule_TheSetting", "yup"}})
        Dim setting As New Setting(Of Boolean)("MyModule_TheSetting", SettingScope.Host, False)
        Dim repository As ISettingsRepository = New SettingsRepository(New ModuleInstanceContext())

        Dim settingValue = repository.GetValue(setting, CollectionExtensions.GetFlexibleBooleanParsingFunction("yup"))

        Assert.IsTrue(settingValue)
    End Sub

    Private Sub StubHostSettings(settings As Dictionary(Of String, String))
        Dim stubHostController = Substitute.For(Of IHostController)()
        stubHostController.GetSettingsDictionary().Returns(settings)
        HostController.RegisterInstance(stubHostController)
    End Sub
End Class
