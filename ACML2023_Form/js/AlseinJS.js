function toUpper(Kume) {
	var index = Kume.selectionStart;
	Kume.value = Kume.value.replace("ı", "I").replace("i", "İ").toUpperCase();
	Kume.selectionStart = index;
	Kume.selectionEnd = index;
}

function UyariBilgilendirme(Baslik, Icerik, Sonuc) {
	$(() => {
		if (Sonuc === undefined) {
			$('#UyariHead').css('background-color', 'transparent');
			$('#UyariBaslik').css('color', '#000');
			$('#UyariKapatButon').css('display', 'none');
		}
		else {
			if (Sonuc) {
				$('#UyariHead').css('background-color', 'darkseagreen');
				$('#UyariBaslik').css('color', '#fff');
			}
			else {
				$('#UyariHead').css('background-color', '#f00');
				$('#UyariBaslik').css('color', '#fff');
			}
			$('#UyariKapatButon').css('display', 'inline-block');
		}


		$('#UyariBaslik').html(Baslik);
		$('#UyariIcerik').html(Icerik);
		$('#Uyari').modal('show');
	});
}