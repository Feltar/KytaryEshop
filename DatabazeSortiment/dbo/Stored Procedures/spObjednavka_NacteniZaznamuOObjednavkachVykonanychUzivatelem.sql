CREATE PROCEDURE [dbo].[spObjednavka_NacteniZaznamuOObjednavkachVykonanychUzivatelem]
	@IdUzivatel nvarchar(450)
AS
	SELECT 
		*
	FROM 
		[dbo].[Objednavka] as oa
    WHERE 
		IdUzivatel = @IdUzivatel;