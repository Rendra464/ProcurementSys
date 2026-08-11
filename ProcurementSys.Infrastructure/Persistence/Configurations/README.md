Taruh IEntityTypeConfiguration<T> per entity di sini, contoh:
- MaterialRequestConfiguration.cs (max length, index unique RequestNumber)
- ProcurementRequestConfiguration.cs (CHECK constraint Type IN ('Listing','NonListing'))
- AssignmentConfiguration.cs (IsRowVersion, index composite EntityType+EntityId)
- WorkflowStepConfiguration.cs
