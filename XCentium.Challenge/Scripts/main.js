
$(function () {

	initGalleria();


	// initialize Galleria plugin if container is found on page
	function initGalleria() {
		var trigger = $('.galleria');

		if (!trigger.length) { return };

		Galleria.loadTheme('/Content/vendor/galleria-classic/galleria.classic.min.js');
		Galleria.run('.galleria');
	}



}());