CREATE PROCEDURE [dbo].[spPolozkaKosiku_SmazPolozkuZKosiku]
	@IdUzivatel nvarchar(450),
	@IdArtikl int
AS
	DELETE FROM [dbo].[PolozkaKosiku] 
		   WHERE (@IdUzivatel = IdUzivatel) 
							AND 
				 (@IdArtikl = IdArtikl);
	SELECT @@ROWCOUNT;