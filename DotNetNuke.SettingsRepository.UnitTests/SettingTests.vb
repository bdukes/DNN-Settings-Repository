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

    <Test>
    <TestCase("true", True)>
    <TestCase("false", False)>
    Public Sub GetValue_GivenStandardBooleanTrueText_ConvertsSettingValue(settingText As String, expectedValue As Boolean)
        StubHostSettings(New Dictionary(Of String, String) From {{"MyModule_TheSetting", settingText}})
        Dim setting As New Setting(Of Boolean)("MyModule_TheSetting", SettingScope.Host, False)
        Dim repository As ISettingsRepository = New SettingsRepository(New ModuleInstanceContext())

        Dim settingValue = repository.GetValue(setting)

        Assert.AreEqual(expectedValue, settingValue)
    End Sub

    <Test>
    Public Sub GetValue_GivenNoMatchingSetting_ReturnsDefaultSettingValue()
        StubHostSettings(New Dictionary(Of String, String) From {{"MyModule_OtherSetting", "something"}})
        Dim setting As New Setting(Of Boolean)("MyModule_TheSetting", SettingScope.Host, True)
        Dim repository As ISettingsRepository = New SettingsRepository(New ModuleInstanceContext())

        Dim settingValue = repository.GetValue(setting)

        Assert.IsTrue(settingValue)
    End Sub

    <Test>
    Public Sub GetValue_GivenInvalidBooleanText_ThrowsFormatException()
        StubHostSettings(New Dictionary(Of String, String) From {{"MyModule_TheSetting", "not a boolean"}})
        Dim setting As New Setting(Of Boolean)("MyModule_TheSetting", SettingScope.Host, False)
        Dim repository As ISettingsRepository = New SettingsRepository(New ModuleInstanceContext())

        Assert.Throws(Of FormatException)(Function() repository.GetValue(setting))
    End Sub

    Private Sub StubHostSettings(settings As Dictionary(Of String, String))
        Dim stubHostController = Substitute.For(Of IHostController)()
        HostController.RegisterInstance(stubHostController)
        stubHostController = HostController.Instance
        stubHostController.GetSettingsDictionary().Returns(settings)
    End Sub
End Class
