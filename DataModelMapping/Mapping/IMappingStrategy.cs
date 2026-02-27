namespace DataModelMapping.Mapping;

public interface IMappingStrategy
{
    MappingKey Key {get;}
    object Execute(object data);

}