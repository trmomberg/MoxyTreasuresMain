-- --------------------------------------------------------------------------------
-- Name: Todd Momberg
-- Class: Capstone Project
-- Final - MoxyTreasures Website
-- --------------------------------------------------------------------------------
Use [MoxyTreasures]		-- Get out of the master database
SET NOCOUNT ON			-- Report only errors

-- --------------------------------------------------------------------------------
-- Drop Statements
-- --------------------------------------------------------------------------------

--Tables

IF OBJECT_ID( 'TOrderProductList' )				IS NOT NULL		DROP TABLE TOrderProductList
IF OBJECT_ID( 'TCart' )							IS NOT NULL		DROP TABLE TCart
IF OBJECT_ID( 'TImages' )						IS NOT NULL		DROP TABLE TImages
IF OBJECT_ID( 'TProducts' )						IS NOT NULL		DROP TABLE TProducts
IF OBJECT_ID( 'TUserLogins' )					IS NOT NULL		DROP TABLE TUserLogins
IF OBJECT_ID( 'TCategories' )					IS NOT NULL		DROP TABLE TCategories
IF OBJECT_ID( 'TOrders' )						IS NOT NULL		DROP TABLE TOrders
IF OBJECT_ID( 'TUsers' )						IS NOT NULL		DROP TABLE TUsers
IF OBJECT_ID( 'TGenders' )						IS NOT NULL		DROP TABLE TGenders
IF OBJECT_ID( 'TStates' )						IS NOT NULL		DROP TABLE TStates
IF OBJECT_ID( 'TStatuses' )						IS NOT NULL		DROP TABLE TStatuses

-- Stored Procedures
-- Users
IF OBJECT_ID( 'uspSelectUser' )					IS NOT NULL		DROP PROCEDURE uspSelectUser
IF OBJECT_ID( 'uspAddUser' )					IS NOT NULL		DROP PROCEDURE uspAddUser
IF OBJECT_ID( 'uspEditUser' )					IS NOT NULL		DROP PROCEDURE uspEditUser
IF OBJECT_ID( 'uspDeleteUser' )					IS NOT NULL		DROP PROCEDURE uspDeleteUser

-- Categories
IF OBJECT_ID( 'uspSelectCategory' )				IS NOT NULL		DROP PROCEDURE uspSelectCategory
IF OBJECT_ID( 'uspAddCategory' )				IS NOT NULL		DROP PROCEDURE uspAddCategory
IF OBJECT_ID( 'uspEditCategory' )				IS NOT NULL		DROP PROCEDURE uspEditCategory
IF OBJECT_ID( 'uspDeleteCategory' )				IS NOT NULL		DROP PROCEDURE uspDeleteCategory

-- Products
IF OBJECT_ID( 'uspSelectProduct' )				IS NOT NULL		DROP PROCEDURE uspSelectProduct
IF OBJECT_ID( 'uspSelectProductState' )			IS NOT NULL		DROP PROCEDURE uspSelectProductState
IF OBJECT_ID( 'uspAddProduct' )					IS NOT NULL		DROP PROCEDURE uspAddProduct
IF OBJECT_ID( 'uspEditProduct' )				IS NOT NULL		DROP PROCEDURE uspEditProduct
IF OBJECT_ID( 'uspEditProductBuyer' )			IS NOT NULL		DROP PROCEDURE uspEditProductBuyer
IF OBJECT_ID( 'uspDeleteProduct' )				IS NOT NULL		DROP PROCEDURE uspDeleteProduct

-- Orders
IF OBJECT_ID( 'uspSelectOrder' )				IS NOT NULL		DROP PROCEDURE uspSelectOrder
IF OBJECT_ID( 'uspSelectOrderList' )			IS NOT NULL		DROP PROCEDURE uspSelectOrderList
IF OBJECT_ID( 'uspAddOrder' )					IS NOT NULL		DROP PROCEDURE uspAddOrder
IF OBJECT_ID( 'uspEditOrder' )					IS NOT NULL		DROP PROCEDURE uspEditOrder

-- Images
IF OBJECT_ID( 'uspSelectImage' )				IS NOT NULL		DROP PROCEDURE uspSelectImage
IF OBJECT_ID( 'uspSelectProductPrimaryImage' )	IS NOT NULL		DROP PROCEDURE uspSelectProductPrimaryImage
IF OBJECT_ID( 'uspEditImage' )					IS NOT NULL		DROP PROCEDURE uspEditImage
IF OBJECT_ID( 'uspAddImage' )					IS NOT NULL		DROP PROCEDURE uspAddImage
IF OBJECT_ID( 'uspDeleteImage' )				IS NOT NULL		DROP PROCEDURE uspDeleteImage

-- Misc
IF OBJECT_ID( 'uspLogin' )						IS NOT NULL		DROP PROCEDURE uspLogin
IF OBJECT_ID( 'uspToggleCart' )					IS NOT NULL		DROP PROCEDURE uspToggleCart
IF OBJECT_ID( 'uspGetCart' )					IS NOT NULL		DROP PROCEDURE uspGetCart
IF OBJECT_ID( 'uspIsWatched' )					IS NOT NULL		DROP PROCEDURE uspIsWatched

-- --------------------------------------------------------------------------------
-- Create Tables
-- --------------------------------------------------------------------------------
CREATE TABLE TUsers
(
	 intUserID			INTEGER					NOT NULL
	,strFirstName		VARCHAR(50)				NOT NULL
	,strLastName		VARCHAR(50)				NOT NULL
	,strEmailAddress	VARCHAR(50)				NOT NULL
	,dtmDateOfBirth		DATETIME				NOT NULL
	,strCity			VARCHAR(50)				NOT NULL
	,intStateID			INTEGER					NOT NULL  
	,intGenderID		INTEGER					NOT NULL
	,strZipCode			VARCHAR(50)				NOT NULL
	,blnAdmin			INTEGER					NOT NULL
	,CONSTRAINT TUsers_PK PRIMARY KEY ( intUserID )
)

CREATE TABLE TUserLogins
(
	 intUserID			INTEGER					NOT NULL
	,strUserName		VARCHAR(50)				NOT NULL
	,biPasswordHash		BINARY(64)				NOT NULL
	,uiSalt				UNIQUEIDENTIFIER		NOT NULL
	,CONSTRAINT TUserLogins_PK PRIMARY KEY ( intUserID )
)

CREATE TABLE TProducts
(
	 intProductID		INTEGER					NOT NULL
	,intCategoryID		INTEGER					NOT NULL
	,strTitle			VARCHAR(50)				NOT NULL
	,strDescription		VARCHAR(250)			NOT NULL
	,intPrice			INTEGER					NOT NULL
	,CONSTRAINT TProducts_PK PRIMARY KEY ( intProductID )
)

CREATE TABLE TStates
(
	 intStateID			INTEGER					NOT NULL
	,strState			VARCHAR(50)				NOT NULL
	,CONSTRAINT TStates_PK PRIMARY KEY ( intStateID )
)

CREATE TABLE TImages
(
	 intImageID			INTEGER					NOT NULL
	,intProductID		INTEGER					NULL
	,blnPrimaryImage	BIT						NOT NULL
	,ImageData			VARBINARY(max)			NOT NULL
	,strFileName		VARCHAR(255)			NOT NULL
	,strImageType		VARCHAR(255)			NOT NULL
	,intImageSize		INTEGER					NOT NULL
	,CONSTRAINT TImages_PK PRIMARY KEY ( intImageID )
)

CREATE TABLE TCart
(
	 intUserID			INTEGER					NOT NULL
	,intProductID		INTEGER					NOT NULL
	,CONSTRAINT TCart_PK PRIMARY KEY ( intProductID,  intUserID )
)

CREATE TABLE TOrders
(
	intOrderID			INTEGER					NOT NULL
   ,intUserID			INTEGER					NOT NULL
   ,intTotal			INTEGER					NOT NULL
   ,dtmDateOfOrder		DATETIME				NOT NULL
   ,strShippingAddress	VARCHAR(50)				NOT NULL
   ,strCity				VARCHAR(50)				NOT NULL
   ,intStateID			INTEGER					NOT NULL
   ,intStatusID			INTEGER					NOT NULL
   ,CONSTRAINT TOrders_PK PRIMARY KEY (intOrderID)
)

CREATE TABLE TOrderProductList
(
	 intOrderID			INTEGER					NOT NULL
	,intProductID		INTEGER					NOT NULL
	,CONSTRAINT TOrderProductList_PK PRIMARY KEY (intOrderID, intProductID)
)

CREATE TABLE TGenders
(
	 intGenderID		INTEGER					NOT NULL
	,strGender			VARCHAR(50)				NOT NULL
	,CONSTRAINT TGenders_PK PRIMARY KEY ( intGenderID )
)

CREATE TABLE TCategories
(
	 intCategoryID		INTEGER					NOT NULL
	,strCategory		VARCHAR(50)				NOT NULL
	,CONSTRAINT TCategories_PK PRIMARY KEY ( intCategoryID )
)

CREATE TABLE TStatuses
(
	 intStatusID		INTEGER					NOT NULL
	,strStatus			VARCHAR(50)				NOT NULL
	,CONSTRAINT TStatuses_PK PRIMARY KEY ( intStatusID )
)


-- --------------------------------------------------------------------------------
-- Identify and Create Foreign Keys  
-- --------------------------------------------------------------------------------
--
-- #	Child								Parent						Column(s)
-- -	-----								------						---------
-- 1	TImages								TProducts					intProductID
-- 2	TCart								TUsers						intUserID
-- 3    TCart								TProducts					intProductID
-- 4	TUsers								TGenders					intGenderID
-- 5	TOrders								TUsers						intUserID
-- 6    TOrders								TState						intStateID
-- 7    TOrderProductList					TOrders						intOrderID
-- 8    TOrderProductList					TProducts					intProductID
-- 9	TProducts							TCategories					intCategoryID
-- 10	TOrders								TStatuses					intStatusID

-- 1
ALTER TABLE TImages ADD CONSTRAINT TImages_TProducts_FK
FOREIGN KEY ( intProductID ) REFERENCES TProducts ( intProductID )

-- 2
ALTER TABLE TCart ADD CONSTRAINT TCart_TUsers_FK
FOREIGN KEY ( intUserID ) REFERENCES TUsers ( intUserID )

-- 3
ALTER TABLE TCart ADD CONSTRAINT TCart_TProducts_FK
FOREIGN KEY ( intProductID ) REFERENCES TProducts ( intProductID )

-- 4
ALTER TABLE TUsers ADD CONSTRAINT TUsers_TGenders_FK
FOREIGN KEY ( intGenderID ) REFERENCES TGenders ( intGenderID )

-- 5
ALTER TABLE TOrders ADD CONSTRAINT TOrders_TUsers_FK
FOREIGN KEY ( intUserID ) REFERENCES TUsers ( intUserID )

-- 6
ALTER TABLE TOrders ADD CONSTRAINT TOrders_TStates_FK
FOREIGN KEY ( intStateID ) REFERENCES TStates ( intStateID )

-- 7
ALTER TABLE TOrderProductList ADD CONSTRAINT TOrderProductList_TOrders_FK
FOREIGN KEY ( intOrderID ) REFERENCES TOrders ( intOrderID )

-- 8
ALTER TABLE TOrderProductList ADD CONSTRAINT TOrderProductList_TProducts_FK
FOREIGN KEY ( intProductID ) REFERENCES TProducts ( intProductID )

-- 9
ALTER TABLE TProducts ADD CONSTRAINT TProducts_TCategories_FK
FOREIGN KEY ( intCategoryID ) REFERENCES TCategories ( intCategoryID )

-- 10
ALTER TABLE TOrders ADD CONSTRAINT TOrders_TStatuses_FK
FOREIGN KEY ( intStatusID ) REFERENCES TStatuses ( intStatusID )

-- --------------------------------------------------------------------------------
-- Insert Values
-- --------------------------------------------------------------------------------
INSERT INTO TStates( intStateID, strState)
VALUES ( 1, 'Alabama')
	  ,( 2, 'Alaska')
	  ,( 3, 'Arizona')
	  ,( 4, 'Arkansas')
	  ,( 5, 'California')
	  ,( 6, 'Colorado')
	  ,( 7, 'Connecticut')
	  ,( 8, 'Delaware')
	  ,( 9, 'Florida')
	  ,( 10, 'Georgia')
	  ,( 11, 'Hawaii')
	  ,( 12, 'Idaho')
	  ,( 13, 'Illinois')
	  ,( 14, 'Indiana')
	  ,( 15, 'Iowa')
	  ,( 16, 'Kansas')
	  ,( 17, 'Kentucky')
	  ,( 18, 'Louisiana')
	  ,( 19, 'Maine')
	  ,( 20, 'Maryland')
	  ,( 21, 'Massachusetts')
	  ,( 22, 'Michigan')
	  ,( 23, 'Minnesota')
	  ,( 24, 'Mississippi')
	  ,( 25, 'Missouri')
	  ,( 26, 'Montana')
	  ,( 27, 'Nebraska')
	  ,( 28, 'Nevada')
	  ,( 29, 'New Hampshire')
	  ,( 30, 'New Jersey')
	  ,( 31, 'New Mexico')
	  ,( 32, 'New York')
	  ,( 33, 'North Carolina')
	  ,( 34, 'North Dakota')
	  ,( 35, 'Ohio')
	  ,( 36, 'Oklahoma')
	  ,( 37, 'Oregon')
	  ,( 38, 'Pennsylvania')
	  ,( 39, 'Rhode Island')
	  ,( 40, 'South Carolina')
	  ,( 41, 'South Dakota')
	  ,( 42, 'Tennessee')
	  ,( 43, 'Texas')
	  ,( 44, 'Utah')
	  ,( 45, 'Vermont')
	  ,( 46, 'Virginia')
	  ,( 47, 'Washington')
	  ,( 48, 'West Virginia')
	  ,( 49, 'Wisconsin')
	  ,( 50, 'Wyoming')

INSERT INTO TGenders(intGenderID, strGender)
VALUES (0, 'Male')
	  ,(1, 'Female')
	  ,(2, 'Other')

INSERT INTO TUsers (intUserID, strFirstName, strLastName, strEmailAddress, dtmDateOfBirth, strCity, intstateID, intGenderID,  strZipCode, blnAdmin)
VALUES (0, 'Empty', 'User', '', '01/01/2100', '', 1, 1, '', 0)

INSERT INTO TCategories (intCategoryID, strCategory)
VALUES  (0, '')
	   ,(1, 'Ring')
	   ,(2, 'Necklace')
	   ,(3, 'Earrings')
	   ,(4, 'Bracelet')
	   ,(5, 'Clothing')
	   ,(6, 'Other')

INSERT INTO TStatuses (intStatusID, strStatus)
VALUES  (0, 'Unknown')
	   ,(1, 'Cart')
	   ,(2, 'Ordered')
	   ,(3, 'Complete')


-- --------------------------------------------------------------------------------
-- Stored Procedures
-- --------------------------------------------------------------------------------

-- --------------------------------------------------
-- Users
-- --------------------------------------------------

-- Select User
GO
CREATE PROCEDURE uspSelectUser
	@intUserID  INT = NULL

AS
BEGIN
    SET NOCOUNT ON

	IF @intUserID IS NOT NULL
		SELECT * FROM TUsers WHERE intUserID = @intUserID		
	ELSE
		SELECT * FROM TUsers
		

END 
GO

-- Add User
GO
CREATE PROCEDURE uspAddUser
     @intUserID				INT=0 OUTPUT
	,@strFirstName			VARCHAR(50)
	,@strLastName			VARCHAR(50)
	,@strEmailAddress		VARCHAR(50) 
    ,@strPassword			VARCHAR(50)
	,@dtmDateOfBirth		DATETIME
	,@intStateID			INTEGER
	,@strCity				VARCHAR(50)	
	,@intGenderID			INTEGER
	,@strZipCode			VARCHAR(50)
	,@blnAdmin				BIT
AS
BEGIN
    SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @count TINYINT
	-- Determine if email is already being used
	SELECT @count = count(*) FROM TUsers WHERE strEmailAddress = @strEmailAddress

	if @count>0
		BEGIN
			RETURN 2 -- User already exists
		END
	ELSE 
		BEGIN
			BEGIN TRANSACTION
				SELECT @intUserID = MAX( intUserID ) + 1
				FROM TUsers (TABLOCKX) -- lock table until the end of transaction

					--default to 1 if table is empty
				SELECT @intUserID = COALESCE ( @intUserID, 1 )

					-- Insert User Values
				INSERT INTO TUsers (intUserID, strFirstName, strLastName, strEmailAddress, dtmDateOfBirth, strCity, intstateID, intGenderID, strZipCode, blnAdmin)
				VALUES (@intUserID, @strFirstName, @strLastName, @strEmailAddress, @dtmDateOfBirth, @strCity, @intStateID, @intGenderID, @strZipCode, @blnAdmin)
		
				DECLARE @salt UNIQUEIDENTIFIER=NEWID()
		
				-- Insert Login Values
				INSERT INTO TUserLogins (intUserID, strUserName, biPasswordHash, uiSalt)
				VALUES(@intUserID, @strEmailAddress, HASHBYTES('SHA2_512', @strPassword+CAST(@salt AS NVARCHAR(36))), @salt)
		
			COMMIT TRANSACTION	
			RETURN 1 -- New record created
		END
END
GO

-- Edit User
GO
CREATE PROCEDURE uspEditUser
	 @intUserID				INT OUTPUT
	,@strFirstName			VARCHAR(50) = NULL
	,@strLastName			VARCHAR(50) = NULL
	,@strEmailAddress		VARCHAR(50) = NULL
    ,@strPassword			VARCHAR(50) = NULL
	,@dtmDateOfBirth		DATETIME	= NULL
	,@intStateID			INTEGER		= NULL	
	,@strCity				VARCHAR(50)	= NULL
	,@intGenderID			INTEGER		= NULL
	,@strZipCode			VARCHAR(50)	= NULL

AS
BEGIN
    SET NOCOUNT ON
	
	-- Check Email
	IF @strEmailAddress IS NOT NULL
	BEGIN
		DECLARE @count TINYINT
		DECLARE @current_email VARCHAR(50)

			-- Get the currently saved email address from the user
		SELECT @current_email = strEmailAddress FROM TUsers WHERE intUserID = @intUserID

		-- Determine if the email address is being changed. If so, make sure the new one isnt being used already
		if @current_email <> @strEmailAddress -- The email is being changed
		BEGIN
			-- This gets a count of the number of userrs with the new email address
			SELECT @count = count(*) FROM TUsers WHERE strEmailAddress = @strEmailAddress
			IF @count > 0
			BEGIN
				RETURN 2 -- Email is already being used
			END
		END

		UPDATE TUsers SET strEmailAddress = @strEmailAddress WHERE intUserID = @intUserID
		UPDATE TUserLogins SET strUserName = @strEmailAddress WHERE intUserID = @intUserID
	END

	-- Check First Name
	IF @strFirstName IS NOT NULL
	BEGIN
		UPDATE TUsers SET strFirstName = @strFirstName WHERE intUserID = @intUserID
	END
	-- Check Last Name
	IF @strLastName IS NOT NULL
	BEGIN
		UPDATE TUsers SET strLastName = @strLastName WHERE intUserID = @intUserID
	END

	-- Check Date Of Birth
	IF @strLastName IS NOT NULL
	BEGIN
		UPDATE TUsers SET dtmDateOfBirth = @dtmDateOfBirth WHERE intUserID = @intUserID
	END

	-- Check Date Of Birth
	IF @strLastName IS NOT NULL
	BEGIN
		UPDATE TUsers SET dtmDateOfBirth = @dtmDateOfBirth WHERE intUserID = @intUserID
	END
	
	-- Check State
	IF @strLastName IS NOT NULL
	BEGIN
		UPDATE TUsers SET intStateID = @intStateID WHERE intUserID = @intUserID
	END
	
	-- Check City
	IF @strLastName IS NOT NULL
	BEGIN
		UPDATE TUsers SET strCity = @strCity WHERE intUserID = @intUserID
	END
	
	-- Check Gender
	IF @strLastName IS NOT NULL
	BEGIN
		UPDATE TUsers SET intGenderID = @intGenderID WHERE intUserID = @intUserID
	END

	-- Check Zip
	IF @strZipCode IS NOT NULL
	BEGIN
		UPDATE TUsers SET strZipCode = @strZipCode WHERE intUserID = @intUserID
	END

	-- Check Password
	IF @strPassword IS NOT NULL
	BEGIN
		DECLARE @salt UNIQUEIDENTIFIER=NEWID()
		UPDATE TUserLogins SET biPasswordHash = HASHBYTES('SHA2_512', @strPassword+CAST(@salt AS NVARCHAR(36))) WHERE intUserID = @intUserID
		UPDATE TUserLogins SET uiSalt = @salt
	END
	RETURN 1 -- User Updated
	END
	
GO

-- Delete User
GO
CREATE PROCEDURE uspDeleteUser
	@intUserID	INT

AS
BEGIN
    SET NOCOUNT ON

	DELETE FROM TUsers WHERE intUserID = @intUserID
	DELETE FROM TUserLogins WHERE intUserID = @intUserID

END 
GO

-- --------------------------------------------------
-- Products
-- --------------------------------------------------

-- Select Product
GO
CREATE PROCEDURE uspSelectProduct
	 @intProductID  INT = NULL

AS
BEGIN
    SET NOCOUNT ON

	IF @intProductID IS NOT NULL
		SELECT * FROM TProducts WHERE intProductID = @intProductID
	ELSE
		SELECT * FROM TProducts
	
END 
GO

-- Add Product
GO
CREATE PROCEDURE uspAddProduct
     @intProductID			INT=0 OUTPUT
	,@intCategoryID			INT
	,@strTitle				VARCHAR(50)
	,@strDescription		VARCHAR(50)
	,@intPrice				INT		

AS
BEGIN
    SET NOCOUNT ON

	SET XACT_ABORT ON

	BEGIN TRANSACTION

	SELECT @intProductID = MAX( intProductID ) + 1
		FROM TProducts (TABLOCKX) -- lock table until the end of transaction

		--default to 1 if table is empty
		SELECT @intProductID = COALESCE ( @intProductID, 1 )
		
		INSERT INTO TProducts (intProductID, intCategoryID, strTitle, strDescription, intPrice)
		VALUES (@intProductID, @intCategoryID, @strTitle, @strDescription, @intPrice)

	COMMIT TRANSACTION

	RETURN @intProductID

END

GO

-- Edit Product
GO
CREATE PROCEDURE uspEditProduct
	 @intProductID			INT
	,@intCategoryID			INT = NULL
	,@strTitle				VARCHAR(50) = NULL
	,@strDescription		VARCHAR(50) = NULL
	,@intPrice				INT = NULL

	
AS
BEGIN
    SET NOCOUNT ON

	-- Check Category
	IF @intCategoryID IS NOT NULL
	BEGIN
		UPDATE TProducts SET intCategoryID = @intCategoryID WHERE intProductID = @intProductID
	END

	-- Check Title
	IF @strTitle IS NOT NULL
	BEGIN
		UPDATE TProducts SET  strTitle = @strTitle WHERE intProductID = @intProductID
	END

	-- Check Description
	IF @strDescription IS NOT NULL
	BEGIN
		UPDATE TProducts SET strDescription	= @strDescription WHERE intProductID = @intProductID
	END

	-- Check Price
	IF @intPrice IS NOT NULL
	BEGIN
		UPDATE TProducts SET intPrice = @intPrice WHERE intProductID = @intProductID
	END
	
	RETURN 0 -- Product Updated
END
GO

-- --------------------------------------------------
-- Category
-- --------------------------------------------------

-- Select User
GO
CREATE PROCEDURE uspSelectCategory
	@intCategoryID  INT = NULL

AS
BEGIN
    SET NOCOUNT ON

	IF @intCategoryID IS NOT NULL
		SELECT * FROM TCategories WHERE intCategoryID = @intCategoryID		
	ELSE
		SELECT * FROM TCategories
		

END 
GO

-- Add Category
GO
CREATE PROCEDURE uspAddCategory
     @intCategoryID				INT=0 OUTPUT
	,@strCategory				VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON

	SET XACT_ABORT ON

	BEGIN
		BEGIN TRANSACTION
			SELECT @intCategoryID = MAX( intCategoryID ) + 1
			FROM TCategories (TABLOCKX) -- lock table until the end of transaction

				--default to 1 if table is empty
			SELECT @intCategoryID = COALESCE ( @intCategoryID, 1 )

				-- Insert Category Values
			INSERT INTO TCategories (intCategoryID, strCategory)
			VALUES (@intCategoryID, @strCategory)
		
		COMMIT TRANSACTION	
		RETURN @intCategoryID -- New record created
	END
END
GO

-- Edit Category
GO
CREATE PROCEDURE uspEditCategory
	  @intCategoryID				INT OUTPUT
	 ,@strCategory					VARCHAR(50)

AS
BEGIN
    SET NOCOUNT ON

	-- Check Category Text
	IF @strCategory IS NOT NULL
	BEGIN
		UPDATE TCategories SET strCategory = @strCategory WHERE intCategoryID = @intCategoryID
	END
END
	
GO


-- Delete Category
GO
CREATE PROCEDURE uspDeleteCategory
	@intCategoryID	INT

AS
BEGIN
    SET NOCOUNT ON

	DELETE FROM TCategories WHERE intCategoryID = @intCategoryID

END 
GO
-- --------------------------------------------------
-- Orders
-- --------------------------------------------------

-- Select Order 
GO
CREATE PROCEDURE uspSelectOrder
	  @intOrderID  INT = NULL
	 ,@intUserID  INT = NULL

AS
BEGIN
    SET NOCOUNT ON

	IF @intOrderID IS NOT NULL
		SELECT * FROM TOrders WHERE intOrderID = @intOrderID
	ELSE IF @intUserID IS NOT NULL
		SELECT * FROM TOrders WHERE intUserID = @intUserID
	ELSE
		SELECT * FROM TOrders
END 
GO

-- Select Order List
GO
CREATE PROCEDURE uspSelectOrderList
	 @intOrderID  INT = NULL

AS
BEGIN
    SET NOCOUNT ON

	IF @intOrderID IS NOT NULL
		SELECT * FROM TOrderProductList WHERE intOrderID = @intOrderID
	ELSE
		SELECT * FROM TOrderProductList
	
END 
GO

-- Add Product
GO
CREATE PROCEDURE uspAddOrder
     @intOrderID			INT=0 OUTPUT
	,@intUserID				INT
	,@intTotal				INT
	,@strAddress			VARCHAR(50)
	,@strCity				VARCHAR(50)
	,@intStateID			INT
	,@intStatusID			INT
AS
BEGIN
    SET NOCOUNT ON

	SET XACT_ABORT ON

	BEGIN TRANSACTION

	SELECT @intOrderID = MAX( intOrderID ) + 1
		FROM TOrders (TABLOCKX) -- lock table until the end of transaction

		--default to 1 if table is empty
		SELECT @intOrderID = COALESCE ( @intOrderID, 1 )
		
		INSERT INTO TOrders(intOrderID, intUserID, intTotal, strShippingAddress, strCity, intStateID, intStatusID)
		VALUES (@intOrderID, @intUserID, @intTotal, @strAddress, @strCity, @intStateID, @intStatusID)

	COMMIT TRANSACTION

	RETURN @intOrderID

END

GO

-- Edit Product
GO
CREATE PROCEDURE uspEditOrder
	 @intOrderID		    INT	= NULL
	,@intTotal				INT	= NULL
	,@strAddress			VARCHAR(50)	= NULL
	,@strCity				VARCHAR(50)	= NULL
	,@intStateID			INT	= NULL
	,@intStatusID			INT
	
AS
BEGIN
    SET NOCOUNT ON

	-- Check Total
	IF @intTotal IS NOT NULL
	BEGIN
		UPDATE TOrders SET intTotal = @intTotal WHERE intOrderID = @intOrderID
	END

	-- Check Address
	IF @strAddress IS NOT NULL
	BEGIN
		UPDATE TOrders SET  strShippingAddress = @strAddress WHERE intOrderID = @intOrderID
	END

	-- Check City
	IF @strCity IS NOT NULL
	BEGIN
		UPDATE TOrders SET strCity	= @strCity WHERE intOrderID = @intOrderID
	END

	-- Check State
	IF @intStateID IS NOT NULL
	BEGIN
		UPDATE TOrders SET intStateID = @intStateID WHERE intOrderID = @intOrderID
	END

	-- Check Status
	IF @intStatusID IS NOT NULL
	BEGIN
		UPDATE TOrders SET intStatusID = @intStatusID WHERE intOrderID = @intOrderID
	END
	
	RETURN 0 -- Order Updated
END
GO

-- --------------------------------------------------
-- Images
-- --------------------------------------------------

-- Select Image
GO
CREATE PROCEDURE uspSelectImage
	 @intImageID INT = NULL
	,@intProductID INT = NULL
AS
BEGIN
    SET NOCOUNT ON

	IF @intImageID IS NOT NULL
		SELECT * FROM TImages WHERE intImageID = @intImageID		
	ELSE IF  @intProductID IS NOT NULL
		SELECT * FROM TImages WHERE intProductID = @intProductID
	
END 
GO

GO
CREATE PROCEDURE uspSelectProductPrimaryImage
	@intProductID INT  
AS
BEGIN
    SET NOCOUNT ON

	SELECT * FROM TImages WHERE intProductID = @intProductID AND blnPrimaryImage = 1	
	
END 
GO

-- Add Image
GO
CREATE PROCEDURE uspAddImage
	 @intImageID		INT=0 OUTPUT 
	,@intProductID		INT 
	,@blnPrimaryImage	BIT
	,@strFileName		VARCHAR(255)
	,@strImageType		VARCHAR(255)
	,@intImageSize		INT
	,@ImageData			VARBINARY(max)
AS
BEGIN
    SET NOCOUNT ON

	SET XACT_ABORT ON

	BEGIN TRANSACTION

	SELECT @intImageID = MAX( intImageID ) + 1
		FROM TImages (TABLOCKX) -- lock table until the end of transaction

		--default to 1 if table is empty
		SELECT @intImageID = COALESCE ( @intImageID, 1 )
		
		INSERT INTO TImages (intImageID, intProductID, blnPrimaryImage, strFileName, intImageSize, strImageType, ImageData)
		VALUES (@intImageID, @intProductID, @blnPrimaryImage, @strFileName, @intImageSize, @strImageType, @ImageData)

	COMMIT TRANSACTION

	RETURN @intImageID

END 
GO

-- Edit Image
GO
CREATE PROCEDURE uspEditImage
	 @intImageID		INT  
	,@intProductID		INT 
	,@blnPrimaryImage	BIT = NULL
	,@strFileName		VARCHAR(255) = NULL
	,@strImageType		VARCHAR(255) = NULL
	,@intImageSize		INT = NULL
	,@ImageData			VARBINARY = NULL
AS
BEGIN
    SET NOCOUNT ON

	-- Check Primary Image
	IF @blnPrimaryImage IS NOT NULL
	BEGIN
		UPDATE TImages SET blnPrimaryImage = @blnPrimaryImage WHERE intImageID = @intImageID
	END

	-- Check File Name
	IF @strFileName IS NOT NULL
	BEGIN
		UPDATE TImages SET  strFileName = @strFileName WHERE intImageID = @intImageID
	END

	-- Check ImageType
	IF @strImageType IS NOT NULL
	BEGIN
		UPDATE TImages SET strImageType	= @strImageType WHERE intImageID = @intImageID
	END

	-- Check ImageSize
	IF @intImageSize IS NOT NULL
	BEGIN
		UPDATE TImages SET intImageSize = @intImageSize WHERE intImageID = @intImageID
	END
	
	IF @ImageData IS NOT NULL
	BEGIN
		UPDATE TImages SET ImageData = @ImageData WHERE intImageID = @intImageID
	END

	RETURN 0 -- Image Updated
END
GO


-- Delete Image
GO
CREATE PROCEDURE uspDeleteImage
	 @intImageID		INT = NULL
	,@intProductID		INT = NULL

AS
BEGIN

	IF @intImageID IS NOT NULL
		DELETE FROM TImages WHERE intImageID = @intImageID
	ELSE IF @intProductID IS NOT NULL
		DELETE FROM TImages WHERE intProductID = @intProductID

END 
GO

-- --------------------------------------------------
-- Misc
-- --------------------------------------------------

-- Login
GO
CREATE PROCEDURE uspLogin
    @strUserName		VARCHAR(254),
    @strPassword		VARCHAR(50), 
    @intResponse		INTEGER=0		OUTPUT

AS
BEGIN

    SET NOCOUNT ON

    DECLARE @userID INT

    IF EXISTS (SELECT TOP 1 intUserID FROM TUserLogins WHERE strUserName=@strUserName)
    BEGIN
        SET @userID=(SELECT intUserID FROM TUserLogins WHERE strUserName=@strUserName AND biPasswordHash=HASHBYTES('SHA2_512', @strPassword+CAST(uiSalt AS NVARCHAR(36))))

       IF(@userID IS NULL)
           SET @intResponse=-1   --Incorrect password
       ELSE 
           SET @intResponse=@userID     --User successfully logged in
    END
    ELSE
       SET @intResponse=-1    --Invalid login

	RETURN @intResponse

END

GO

-- Toggle watch list
GO
CREATE PROCEDURE uspToggleCart
     @intUserID			INTEGER
    ,@intProductID		INTEGER
	,@intResponse		INTEGER=0		OUTPUT
AS
BEGIN
    SET NOCOUNT ON

	SET XACT_ABORT ON
	
		-- check if this record is already in the database
		DECLARE @ProductID INT
		SELECT @ProductID = intProductID FROM TCart WHERE @intUserID = intUserID

		-- if it is already there, delete it
		IF EXISTS (SELECT * FROM TCart WHERE intUserID = @intUserID AND intProductID = @intProductID)
			BEGIN
				DELETE FROM TCart WHERE @intUserID = intUserID AND @intProductID = intProductID 
				SET @intResponse = -1 -- toggled off
			END
		-- otherwise, insert it
		ELSE
			BEGIN 
			BEGIN TRANSACTION
					INSERT INTO TCart(intUserID, intProductID)
					VALUES (@intUserID, @intProductID)
					SET @intResponse = 1 -- toggled on
				COMMIT TRANSACTION
			END

	RETURN @intResponse
END 
GO

GO
CREATE PROCEDURE uspGetCart
     @intUserID			INTEGER

AS
BEGIN
    SET NOCOUNT ON

	SET XACT_ABORT ON

	BEGIN TRANSACTION

		SELECT * FROM TCart WHERE intUserID = @intUserID	

	COMMIT TRANSACTION

END 
GO


GO
CREATE PROCEDURE uspIsWatched
	  @intUserID		INTEGER
	 ,@intProductID		INTEGER
	 ,@intIsWatched		INTEGER = 0 OUTPUT
AS
BEGIN
    SET NOCOUNT ON

	SET XACT_ABORT ON

		IF EXISTS (SELECT * FROM TCart WHERE intUserID = @intUserID AND intProductID = @intProductID)
		BEGIN
			SET @intIsWatched = 1 -- Product is being watched by the user
		END
	
	RETURN @intIsWatched
END 
GO
-- --------------------------------------------------
-- Tests
-- --------------------------------------------------

EXEC uspAddUser
	@strFirstName		= 'Todd',
	@strLastName		= 'Momberg',
	@strEmailAddress	= 'Todd0929971@gmail.com',
	@strPassword		= '12345',
	@strCity			= '',
	@dtmDateOfBirth     = '',
	@intStateID         = 1,
	@intGenderID        = 1,
	@strZipCode		    = '45251',
	@blnAdmin			= 1

EXEC uspAddUser
	@strFirstName		= '1',
	@strLastName		= '2',
	@strEmailAddress	= '123',
	@strPassword		= '321',
	@strCity			= '',
	@dtmDateOfBirth     = '',
	@intStateID         = 1,
	@intGenderID        = 1,
	@strZipCode		    = '',
	@blnAdmin			= 0

SELECT * FROM TUsers
SELECT * FROM TUserLogins
SELECT * FROM TProducts
SELECT * FROM TImages
SELECT * FROM TCart