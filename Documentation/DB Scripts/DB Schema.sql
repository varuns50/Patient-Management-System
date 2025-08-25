USE [master]
GO
/****** Object:  Database [PatientDB]    Script Date: 25-08-2025 13:37:20 ******/
CREATE DATABASE [PatientDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PatientDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\PatientDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'PatientDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\PatientDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [PatientDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PatientDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PatientDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PatientDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PatientDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PatientDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PatientDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [PatientDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PatientDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PatientDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PatientDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PatientDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PatientDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PatientDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PatientDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PatientDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PatientDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [PatientDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PatientDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PatientDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PatientDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PatientDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PatientDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PatientDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PatientDB] SET RECOVERY FULL 
GO
ALTER DATABASE [PatientDB] SET  MULTI_USER 
GO
ALTER DATABASE [PatientDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PatientDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PatientDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PatientDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [PatientDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [PatientDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'PatientDB', N'ON'
GO
ALTER DATABASE [PatientDB] SET QUERY_STORE = OFF
GO
USE [PatientDB]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_GetAge]    Script Date: 25-08-2025 13:37:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   FUNCTION [dbo].[fn_GetAge](@DOB DATE)
RETURNS INT
AS
BEGIN
    RETURN DATEDIFF(YEAR, @DOB, GETDATE());
END
GO
/****** Object:  Table [dbo].[Conditions]    Script Date: 25-08-2025 13:37:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Conditions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PatientConditions]    Script Date: 25-08-2025 13:37:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PatientConditions](
	[PatientId] [int] NOT NULL,
	[ConditionId] [int] NOT NULL,
	[DiagnosedDate] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PatientId] ASC,
	[ConditionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Patients]    Script Date: 25-08-2025 13:37:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Patients](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NOT NULL,
	[DOB] [date] NOT NULL,
	[Gender] [nvarchar](10) NULL,
	[City] [nvarchar](100) NULL,
	[Email] [nvarchar](255) NOT NULL,
	[Phone] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Phone] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Conditions_Name]    Script Date: 25-08-2025 13:37:20 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Conditions_Name] ON [dbo].[Conditions]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PatientConditions_ConditionId]    Script Date: 25-08-2025 13:37:20 ******/
CREATE NONCLUSTERED INDEX [IX_PatientConditions_ConditionId] ON [dbo].[PatientConditions]
(
	[ConditionId] ASC,
	[DiagnosedDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Patients_City_Gender_DOB]    Script Date: 25-08-2025 13:37:20 ******/
CREATE NONCLUSTERED INDEX [IX_Patients_City_Gender_DOB] ON [dbo].[Patients]
(
	[City] ASC,
	[Gender] ASC,
	[DOB] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PatientConditions] ADD  DEFAULT (getdate()) FOR [DiagnosedDate]
GO
ALTER TABLE [dbo].[PatientConditions]  WITH CHECK ADD  CONSTRAINT [FK_PatientConditions_Conditions] FOREIGN KEY([ConditionId])
REFERENCES [dbo].[Conditions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PatientConditions] CHECK CONSTRAINT [FK_PatientConditions_Conditions]
GO
ALTER TABLE [dbo].[PatientConditions]  WITH CHECK ADD  CONSTRAINT [FK_PatientConditions_Patients] FOREIGN KEY([PatientId])
REFERENCES [dbo].[Patients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PatientConditions] CHECK CONSTRAINT [FK_PatientConditions_Patients]
GO
ALTER TABLE [dbo].[Patients]  WITH CHECK ADD CHECK  (([Gender]='Other' OR [Gender]='Female' OR [Gender]='Male'))
GO
/****** Object:  StoredProcedure [dbo].[sp_AddCondition]    Script Date: 25-08-2025 13:37:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_AddCondition]
    @Name NVARCHAR(100),
    @Description NVARCHAR(255) = NULL
AS
BEGIN
    INSERT INTO Conditions (Name, Description)
    OUTPUT INSERTED.Id
    VALUES (@Name, @Description);
END
GO
/****** Object:  StoredProcedure [dbo].[sp_AddPatient]    Script Date: 25-08-2025 13:37:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Insert Patient
CREATE   PROCEDURE [dbo].[sp_AddPatient]
    @FirstName NVARCHAR(100),
    @LastName NVARCHAR(100),
    @DOB DATE,
    @Gender NVARCHAR(10) = NULL,
    @City NVARCHAR(100) = NULL,
    @Email NVARCHAR(255),
    @Phone NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Patients (FirstName, LastName, DOB, Gender, City, Email, Phone)
    OUTPUT INSERTED.Id
    VALUES (@FirstName, @LastName, @DOB, @Gender, @City, @Email, @Phone);
END
GO
/****** Object:  StoredProcedure [dbo].[sp_AddPatientCondition]    Script Date: 25-08-2025 13:37:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_AddPatientCondition]
    @PatientId     INT,
    @ConditionId   INT,
    @DiagnosedDate DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @DiagnosedDate IS NULL
        SET @DiagnosedDate = CAST(GETDATE() AS DATE);

    -- Prevent duplicates (aligned with PK on (PatientId, ConditionId))
    IF NOT EXISTS (
        SELECT 1
        FROM dbo.PatientConditions WITH (UPDLOCK, HOLDLOCK)
        WHERE PatientId = @PatientId
          AND ConditionId = @ConditionId
    )
    BEGIN
        INSERT INTO dbo.PatientConditions (PatientId, ConditionId, DiagnosedDate)
        VALUES (@PatientId, @ConditionId, @DiagnosedDate);
        -- rows affected = 1
    END
    -- else: rows affected = 0 (no-op)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeletePatient]    Script Date: 25-08-2025 13:37:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Delete Patient
CREATE   PROCEDURE [dbo].[sp_DeletePatient]
    @Id INT
AS
BEGIN
    DELETE FROM Patients WHERE Id = @Id;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllConditions]    Script Date: 25-08-2025 13:37:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_GetAllConditions]
AS
BEGIN
    SELECT Id, Name, Description FROM Conditions;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetConditionById]    Script Date: 25-08-2025 13:37:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_GetConditionById]
    @Id INT
AS
BEGIN
    SELECT Id, Name, Description FROM Conditions WHERE Id = @Id;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetPatientById]    Script Date: 25-08-2025 13:37:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Patient by Id
CREATE   PROCEDURE [dbo].[sp_GetPatientById]
    @Id INT
AS
BEGIN
    SELECT Id, FirstName, LastName, DOB, Gender, City, Email, Phone
    FROM Patients
    WHERE Id = @Id;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_SearchPatients]    Script Date: 25-08-2025 13:37:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_SearchPatients]
    @City NVARCHAR(100) = NULL,
    @Gender NVARCHAR(10) = NULL,
    @MinAge INT = NULL,
    @MaxAge INT = NULL,
    @ConditionId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT p.Id, p.FirstName, p.LastName, p.DOB, p.Gender, p.City, p.Email, p.Phone
    FROM Patients p
    LEFT JOIN PatientConditions pc ON pc.PatientId = p.Id
    WHERE (@City IS NULL OR p.City = @City)
      AND (@Gender IS NULL OR p.Gender = @Gender)
      AND (@MinAge IS NULL OR dbo.fn_GetAge(p.DOB) >= @MinAge)
      AND (@MaxAge IS NULL OR dbo.fn_GetAge(p.DOB) <= @MaxAge)
      AND (@ConditionId IS NULL OR pc.ConditionId = @ConditionId);
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdatePatient]    Script Date: 25-08-2025 13:37:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Update Patient
CREATE   PROCEDURE [dbo].[sp_UpdatePatient]
    @Id INT,
    @FirstName NVARCHAR(100),
    @LastName NVARCHAR(100),
    @DOB DATE,
    @Gender NVARCHAR(10) = NULL,
    @City NVARCHAR(100) = NULL,
    @Email NVARCHAR(255),
    @Phone NVARCHAR(20)
AS
BEGIN
    UPDATE Patients
    SET FirstName = @FirstName,
        LastName = @LastName,
        DOB = @DOB,
        Gender = @Gender,
        City = @City,
        Email = @Email,
        Phone = @Phone
    WHERE Id = @Id;
END
GO
USE [master]
GO
ALTER DATABASE [PatientDB] SET  READ_WRITE 
GO
