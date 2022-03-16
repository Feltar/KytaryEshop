CREATE PROCEDURE [dbo].[spPolozkaKosiku_ZjistiKvantitu]
	@IdUzivatel nvarchar(450),
	@IdArtikl int
AS
	SELECT PocetKusu 
	FROM [dbo].[PolozkaKosiku] 
	WHERE	((@IdUzivatel = IdUzivatel) 
				AND 
			(@IdArtikl = IdArtikl))