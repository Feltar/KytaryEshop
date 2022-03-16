CREATE PROCEDURE [dbo].[spArtikl_ZmenPocetKusuO]
	@IdArtikl int,
	@OKolikKusu  int
AS
	UPDATE 
		dbo.Artikl 
	SET 
		KusuNaSklade = KusuNaSklade+ @OKolikKusu
	WHERE 
		IdArtikl = @IdArtikl;
	SELECT @@ROWCOUNT;