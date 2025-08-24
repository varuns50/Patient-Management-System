----------------------------------------------------------
-- 1. Fetch all patients diagnosed in the last 6 months
----------------------------------------------------------
SELECT p.Id, p.FirstName, p.LastName, p.DOB, p.Gender, p.City, p.Email, p.Phone
FROM Patients p
JOIN PatientConditions pc ON p.Id = pc.PatientId
WHERE pc.DiagnosedDate >= DATEADD(MONTH, -6, GETDATE());

----------------------------------------------------------
-- 2. Get the top 3 cities with maximum patients
----------------------------------------------------------
SELECT TOP 3 City, COUNT(*) AS PatientCount
FROM Patients
GROUP BY City
ORDER BY PatientCount DESC;

----------------------------------------------------------
-- 3. Find patients who have more than 2 conditions
----------------------------------------------------------
SELECT p.Id, p.FirstName, p.LastName, COUNT(pc.ConditionId) AS ConditionCount
FROM Patients p
JOIN PatientConditions pc ON p.Id = pc.PatientId
GROUP BY p.Id, p.FirstName, p.LastName
HAVING COUNT(pc.ConditionId) > 2;

----------------------------------------------------------
-- 4. Stored Procedure: Dynamic patient search
--    Filters: Age range, ConditionId, City
----------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_SearchPatientsDynamic
    @MinAge INT = NULL,
    @MaxAge INT = NULL,
    @ConditionId INT = NULL,
    @City NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT p.Id, p.FirstName, p.LastName, p.DOB, p.Gender, p.City, p.Email, p.Phone
    FROM Patients p
    LEFT JOIN PatientConditions pc ON p.Id = pc.PatientId
    WHERE (@City IS NULL OR p.City = @City)
      AND (@ConditionId IS NULL OR pc.ConditionId = @ConditionId)
      AND (@MinAge IS NULL OR DATEDIFF(YEAR, p.DOB, GETDATE()) >= @MinAge)
      AND (@MaxAge IS NULL OR DATEDIFF(YEAR, p.DOB, GETDATE()) <= @MaxAge);
END;

----------------------------------------------------------
-- 5. Average age of patients grouped by condition
----------------------------------------------------------
SELECT c.Name AS ConditionName,
       AVG(DATEDIFF(YEAR, p.DOB, GETDATE())) AS AvgAge
FROM Patients p
JOIN PatientConditions pc ON p.Id = pc.PatientId
JOIN Conditions c ON pc.ConditionId = c.Id
GROUP BY c.Name;
