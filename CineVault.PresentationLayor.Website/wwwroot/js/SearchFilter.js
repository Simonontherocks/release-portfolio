// Deze code zal ervoor zorgen dat een zoekbalk wordt getoond waarin er live gefilterd kan worden.
// Bij een niet-gevonden resultaat zal een melding worden getoont.

	$(document).ready(function () { // Zorgt ervoor dat dit script pas werkt nadat de volledige HTML is ingeladen

		$("#searchInput").on("keyup", function () { // Voer deze functie uit telkens de gebruiker een toets loslaat in het zoekveld
			let value = $(this).val().toLowerCase(); // Haal de huidige waarde van het zoekveld op en zet die om naar kleine letters
			const rows = $("table tbody tr"); // Selecteer alle rijen in de <tbody> van de tabel
			let visibleRows = 0; // Tel hoeveel rijen zichtbaar blijven na filtering

			rows.each(function () { // Voor elke rij in de tabel:
				let isVisible = $(this).text().toLowerCase().indexOf(value) > -1; // Bepaal of de rij de zoekterm bevat
				$(this).toggle(isVisible); // Verberg of toon de rij op basis van of hij de zoekterm bevat
				if (isVisible) visibleRows++; // Verhoog de teller als de rij zichtbaar is
			});

			if (visibleRows === 0) { // Toon of verberg de melding afhankelijk van of er zichtbare rijen zijn
				$("#noResultsMessage").show();  // Toon melding
			} else {
				$("#noResultsMessage").hide();  // Verberg melding
			}
		});
	});