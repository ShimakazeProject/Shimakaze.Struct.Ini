# IniDocumentHelper.ParseAsync Method
Namespace: [Shimakaze.Struct.Ini](Shimakaze.Struct.Ini/Shimakaze.Struct.Ini.md)  
Assembly: Shimakaze.Struct.Ini.dll  

# Overloads
||
|-|-|
[ParseAsync(string s)](Shimakaze.Struct.Ini.Helper/IniDocumentHelper/Methods/ParseAsync?id=parseasyncstring-s)
[ParseAsync(Stream s)](Shimakaze.Struct.Ini.Helper/IniDocumentHelper/Methods/ParseAsync?id=parseasyncstream-s)

## ParseAsync(string s)
```csharp
public static async Task<IniDocument> ParseAsync(string s);
```

### Parameters
`s` [String](//docs.microsoft.com/zh-cn/dotnet/api/system.string)

### Returns
[Task](https://docs.microsoft.com/dotnet/api/system.threading.tasks.task-1)<[IniDocument](Shimakaze.Struct.Ini/IniDocument/IniDocument.md)>

## ParseAsync(Stream s)
```csharp
public static async Task<IniDocument> ParseAsync(Stream s);
```

### Parameters
`s` [Stream](//docs.microsoft.com/zh-cn/dotnet/api/system.io.stream)

### Returns
[Task](https://docs.microsoft.com/dotnet/api/system.threading.tasks.task-1)<[IniDocument](Shimakaze.Struct.Ini/IniDocument/IniDocument.md)>
