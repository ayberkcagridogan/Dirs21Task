# DataModelMapping

DataModelMapping is a generic, extensible JSON-to-Model mapping engine designed with a pipeline-based architecture and strict separation of concerns.

The system focuses on:
- Strong typing
- Structured validation
- Safe enum handling
- Result-based flow (no exception-driven logic)
- Clean Architecture principles
- High testability
  
---

# 1. System Overview

The system transforms JSON payloads into strongly-typed domain models using a validation-first pipeline.

Processing Flow:

JSON Input  
→ JsonValidationPipeline<T>  
  • JSON Deserialization  
  • Required property validation  
  • Business rule validation (FluentValidation)  
→ IMapper implementation  
→ Result<T> output  

Validation is fully completed before mapping is executed.

---

# 2. System Architecture

The architecture is divided into three main responsibilities:

1. Validation Pipeline  
2. Mapping Logic  
3. Orchestration  

This ensures strict separation of concerns and extensibility.

## 2.1 IJsonValidationPipeline<T>

Responsible for:

- Deserializing JSON into the source model
- Checking required properties
- Executing FluentValidation rules
- Returning structured validation errors
- Producing a validated model if successful

This component guarantees that only valid data reaches the mapping layer.

Example responsibility:

```csharp
public interface IJsonValidationPipeline<T>
{
    Task<Result<T>> ValidationAsync(string json, CancellationToken cancellationToken = default);
}
```

## 2.2 IMappingStrategy (Strategy Contract)

Responsible for transformation logic and execute validation pipeline.

Each mapper:
- Locates the correct IJsonValidationPipeline<T> implementation
- Executes validation
- Applies transformation logic
- Computes derived properties if needed
- Returns Result<object>

Mapping does not contain validation logic.

Example:
```csharp
public interface IMappingStrategy
{
    MappingKey Key {get;}
    Task<Result<object>> ExecuteAsync (object data, CancellationToken cancellationToken = default);

}
```

## 2.3 MappingHandler (Orchestrator)

Acts as the central coordinator.

Responsibilities:
- Resolves source and target types
- Locates the correct IMapper implementation
- Executes mapping
- Returns final Result<object>

MappingHandler does not contain validation or mapping logic.

Key Method:

```csharp
Task<Result<object>> Map(
    object data,
    string sourceType,
    string targetType,
    CancellationToken cancellationToken = default);
```

## 2.4 Mapper Resolution Strategy

The system uses a key-based mapper registry.
- Each mapper inherits from IMapper
- Each mapper defines a unique constant Key
- While creating MapHandler, all mappers are registered
- A dictionary is created: Dictionary<MappingKey, IMapper>
- MappingHandler resolves the correct mapper using the provided key

## 2.5 Enum Safety

Enums are parsed using safe extension methods.

Features:
- Case-insensitive parsing
- Validation against undefined enum values
- Result-based failure handling

Example:
```csharp
  var roomTypeResult = bookingReservation.RoomType.ToEnum<RoomType>();
  if(roomTypeResult.IsFailed)
      return Result.Fail(roomTypeResult.Errors.Select(x => x.Message));
```
Direct use of Enum.Parse is avoided.

---

# 3. Extending the System

## 3.1 Adding a New Mapping

To add a new mapping:
- Create the source model if needs
- Create the target model if needs
- Implement IMapper with MappingKey (public MappingKey Key => new("Booking.Reservation", "Model.Reservation");) 
- Ensure validation rules exist for TSource

No changes are required in MappingHandler.

## 3.2 Adding Validation Rules

To extend validation:
- Implement or update FluentValidation rules or TSource
- Validation will automatically execute in the pipeline

---

# 4. Assumptions

The following assumptions were made:
- JSON input structure matches the source model schema.
- Each source-target pair has exactly one mapper implementation.
- Validation must complete successfully before mapping.
- Enum values are matched by name (case-insensitive).
- The system operates in a backend-controlled environment.

---

# 5. Limitations

- Reflection-based type resolution may affect performance under heavy load.
- Deep nested object mapping requires additional mapper implementations.

---

# 6. Testing Strategy

The system is tested for:
- Successful mapping scenarios
- Invalid enum values
- Invalid JSON format
- Required field violations
- Business rule validation failures

Testing ensures predictable behavior under both success and failure conditions.

---

# 7. Design Principles

- Pipeline-based validation
- Strict separation of concerns
- Dependency Injection driven
- Result-based error handling
- Async-first design
- SOLID-compliant architecture
- High testability

---

# 8. Future Improvements

- Reflection caching
- Source generator-based mapping
- Performance benchmarking
- Advanced nested mapping strategies
- Configurable mapping profiles


# 9. How to use MappingHandler

This section explains how to integrate and use the DataModelMapping system.

## 9.1 Register Services

Ensure all mappers and validation pipelines are registered in the DI container.

Example:

```csharp
    var serviceProvider = MappingServiceCollectionExtensions.Create();
    _mappingHandler = serviceProvider.GetRequiredService<MappingHandler>();
```
or get MappingHandler with Constructor Dependency Injection 

## 9.2 Prepare JSON Input

Provide the source data as a JSON string.
```json
{
  "id": 1,
  "hotelName": "Grand Berlin Hotel",
  "reservationTime": "2026-02-28T14:30:00",
  "checkIn": "2026-03-10T15:00:00",
  "stayDays": 5,
  "numberOfPerson": 3,
  "roomType": "Deluxe",
  "price": 750
}
```

## 9.3 Execute Mapping

Use MappingHandler to perform validation and mapping.
```csharp
    var result = await handler.Map(
      jsonData,
      "Model.Reservation",
      "Booking.Reservation",
      cancellationToken);
```

## 9.4 Handle Result

The system uses a Result-based pattern.
```csharp
    if (result.IsSuccess)
    {
       var mappedModel = result.Value;
    } 
    else
    {
      var errors = result.Errors;
    }
```
Mapping is only executed if validation succeeds.
