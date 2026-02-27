namespace DataModelMapping.Mapping;

public readonly record struct MappingKey
(   
    MappingType From,
    MappingType To
);