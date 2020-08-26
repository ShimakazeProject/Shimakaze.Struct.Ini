# IniSection Struct
Namespace Shimakaze.Struct.Ini  
Assembly: Shimakaze.Struct.Ini.dll  

表示一个ini文档

```csharp
public struct IniSection
```
Inheritance [Object](//docs.microsoft.com/zh-cn/dotnet/api/system.object) -> [ValueType](//docs.microsoft.com/zh-cn/dotnet/api/system.valuetype) -> IniSection  

## Properties
||
|-|-|
[Content](Shimakaze.Struct.Ini/IniSection/Properties/Content.md)|Section对象中的所有[IniKeyValuePair](../IniKeyValuePair/IniKeyValuePair.md)对象
[Item[String]](Shimakaze.Struct.Ini/IniSection/Properties/Item[].md)|获取指定键的[IniKeyValuePair](../IniKeyValuePair/IniKeyValuePair.md)对象
[Name](Shimakaze.Struct.Ini/IniSection/Properties/Name.md)|取得Section的名字
[Summary](Shimakaze.Struct.Ini/IniSection/Properties/Summary.md)|取得Section名称行后面的注释

## Methods
||
|:-|:-|
[Equals(object obj)](//docs.microsoft.com/dotnet/api/system.object.equals)|Determines whether the specified object is equal to the current object. <br>(Inherited from Object)
[GetHashCode()](//docs.microsoft.com/zh-cn/dotnet/api/system.object.gethashcode)|Serves as the default hash function. <br>(Inherited from Object)
[ToString()](Shimakaze.Struct.Ini/IniSection/Methods/ToString.md)|转换为常见的键值对形式
[TryGetKey](Shimakaze.Struct.Ini/IniSection/Methods/TryGetKey.md)|尝试获取键
## See also
- [IniSection]()
- [IniKeyValue]()