# IniDocument Struct
Namespace: [Shimakaze.Struct.Ini](Shimakaze.Struct.Ini/Shimakaze.Struct.Ini.md)  
Assembly: Shimakaze.Struct.Ini.dll  

表示一个ini文档

```csharp
public struct IniDocument
```
Inheritance [Object](//docs.microsoft.com/dotnet/api/system.object) -> [ValueType](//docs.microsoft.com/dotnet/api/system.valuetype) -> IniDocument  

# Properties
||
|-|-|
[Item[String]](Shimakaze.Struct.Ini/IniDocument/Properties/Item[].md)|获取指定名称的Section对象
[NoSectionContent](Shimakaze.Struct.Ini/IniDocument/Properties/NoSectionContent.md)|此实例所有的未存放在Section中的[IniKeyValuePair](Shimakaze.Struct.Ini/IniKeyValuePair/IniKeyValuePair.md)对象
[Sections](Shimakaze.Struct.Ini/IniDocument/Properties/Sections.md)|此实例所有的Section

# Methods
||
|:-|:-|
[Equals(object obj)](//docs.microsoft.com/dotnet/api/system.object.equals)|Determines whether the specified object is equal to the current object. <br>(Inherited from Object)
[GetFromNoSectionContent(string key)](Shimakaze.Struct.Ini/IniDocument/Methods/GetFromNoSectionContent.md)|从NoSectionContent中获取对象
[GetHashCode()](//docs.microsoft.com/zh-cn/dotnet/api/system.object.gethashcode?view=dotnet-plat-ext-3.1#System_Object_GetHashCode)|Serves as the default hash function. <br>(Inherited from Object)
[ToString()](Shimakaze.Struct.Ini/IniDocument/Methods/ToString.md)|转换为INI字符串对象

# See also
- [IniSection](Shimakaze.Struct.Ini/IniSection/IniSection.md)
- [IniKeyValuePair](Shimakaze.Struct.Ini/IniKeyValuePair/IniKeyValuePair.md)