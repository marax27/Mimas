/****** Object:  Database [MimasDatabase]    Script Date: 2022-07-07 23:55:34 ******/

/*** The scripts of database scoped configurations in Azure should be executed inside the target database connection. ***/
GO
-- ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 8;
GO
/****** Object:  Table [dbo].[Boxes]    Script Date: 2022-07-07 23:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Boxes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[short_id] [varchar](50) NOT NULL,
	[owner_id] [uniqueidentifier] NOT NULL,
	[registered_on] [datetime] NOT NULL,
	[delivered_on] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[short_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Items]    Script Date: 2022-07-07 23:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Items](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](256) NOT NULL,
	[item_count] [int] NOT NULL,
	[owner_id] [uniqueidentifier] NOT NULL,
	[box_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Owners]    Script Date: 2022-07-07 23:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Owners](
	[id] [uniqueidentifier] NOT NULL,
	[name] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Items] ADD  DEFAULT (NULL) FOR [box_id]
GO
ALTER TABLE [dbo].[Owners] ADD  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[Boxes]  WITH CHECK ADD FOREIGN KEY([owner_id])
REFERENCES [dbo].[Owners] ([id])
GO
ALTER TABLE [dbo].[Items]  WITH CHECK ADD FOREIGN KEY([box_id])
REFERENCES [dbo].[Boxes] ([id])
GO
ALTER TABLE [dbo].[Items]  WITH CHECK ADD FOREIGN KEY([owner_id])
REFERENCES [dbo].[Owners] ([id])
GO
