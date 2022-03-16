CREATE PROCEDURE [dbo].[spObjednavkaArtikl_VytvoreniZaznamuVObjednavkaArtikl]
	@IdObjednavka int,
	@IdArtikl int,
	@PocetKusu int,
	@CenaKus int
AS
	INSERT INTO 
		[dbo].[ObjednavkaArtikl] (IdObjednavka, IdArtikl, PocetKusu, CenaKus) 
	VALUES 
		(@IdObjednavka, @IdArtikl, @PocetKusu, @CenaKus);
	SELECT @@ROWCOUNT;