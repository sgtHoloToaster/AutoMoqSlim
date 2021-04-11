# AutoMoqSlim
AutoMoqSlim is a simple "auto-mocking" container inspired by [AutoMoq](https://github.com/darrencauthon/AutoMoq) with less dependencies and compatible with .NET Core

## What is it for?
Suppose you have a class with multiple dependencies
```C#
public ClassForTesting(IMockMe mockMe, INoNeedToMock noNeedToMock, ILogger logger)
```
But for your test you need to mock only one of them. In any case, in order to create an instance of the target class you need to create stubs for all of its dependencies manually
```C#
var mock = new Mock<IMockMe>();
mock.Setup(/*setup*/)
// continue to setup your mock
var stub1 = new Mock<INoNeedToMock>();
var stub2 = new Mock<ILogger>();
var target = new ClassForTesting(mock.Object, stub1.Object, stub2.Object);
```
And the number of unused dependencies may be much higher
This package makes it a little bit easier. In this case it would be something like
 ```C#
 var autoMoqer = new AutoMoqSlim();
 autoMoqer.GetMock<IMockMe>()
  .Setup(/*setup*/)
 // continue to setup your mock
 var target = autoMoqer.Create<ClassForTesting>();
```

## Installation
#### via Package Manager
```powershell
PM> Install-Package AutoMoqSlim
```

#### via dotnet CLI
```bash
> dotnet add package AutoMoqSlim
```

