CREATE PROCEDURE [dbo].[spPolozkaKosiku_NactiPolozkyKosikuUzivatele]
	@IdUzivatel nvarchar(450)
AS
	SELECT 
		Id, IdUzivatel, IdArtikl, PocetKusu
	FROM
		[dbo].PolozkaKosiku
	WHERE 
		IdUzivatel = @IdUzivatel;