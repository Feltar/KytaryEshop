CREATE PROCEDURE [dbo].[spObjednavka_VytvoreniZaznamuVDatabaziObjednavka]
	@IdUzivatel nvarchar(450),
	@Datum DATE
AS
	--Vytvoreni zaznamu v databazi Objednavka
	INSERT INTO 
		dbo.Objednavka (IdUzivatel, Datum) 
	OUTPUT 
		INSERTED.IdObjednavka 
	VALUES 
		(@IdUzivatel, @Datum);
SELECT @@ROWCOUNT;