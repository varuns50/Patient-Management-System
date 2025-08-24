------------------------------------------------------------
-- SQL Optimization: Add index for DiagnosedDate filtering
------------------------------------------------------------

-- Before running, ensure you are using the correct database
USE [PatientDB];
GO

-- Create nonclustered index to optimize 'last 6 months' query
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes 
    WHERE name = 'IX_PatientConditions_DiagnosedDate' 
      AND object_id = OBJECT_ID('dbo.PatientConditions')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_PatientConditions_DiagnosedDate
    ON dbo.PatientConditions (DiagnosedDate)
    INCLUDE (PatientId);
END
GO