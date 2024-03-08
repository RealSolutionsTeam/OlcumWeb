$(function() {
	$('input[type="checkbox"], input[type="radio"]').each(function() {
		if ($(this).closest('[class*="list-"]').is('.list-standard')) {
			if ($(this).is(':checked')) {
				$(this).siblings('[data-icon]').attr('data-prefix', 'fas').closest('.checkbox').addClass('checked');
				if ($(this).is('[type="checkbox"]')) {
					$(this).siblings('[data-icon]').attr('data-icon', 'check-square');
				} else if ($(this).is('[type="radio"]')) {
					$(this).siblings('[data-icon]').attr('data-icon', 'dot-circle').closest('.checkbox').siblings('.checkbox').removeClass('checked').children('[type="radio"]').prop('checked', false).siblings('[data-icon]').attr({'data-icon': 'circle', 'data-prefix': 'far'});
				}
			} else {
				$(this).siblings('[data-icon]').attr('data-prefix', 'far').closest('.checkbox').removeClass('checked');
				if ($(this).is('[type="checkbox"]')) {
					$(this).siblings('[data-icon]').attr('data-icon', 'square');
				} else if ($(this).is('[type="radio"]')) {
					$(this).siblings('[data-icon]').attr('data-icon', 'circle');
				}
			}
		} else if ($(this).closest('[class*="list-"]').is('.list-custom')) {
			if ($(this).is(':checked')) {
				$(this).siblings('[data-icon]').attr('data-prefix', 'fas').closest('.checkbox').addClass('checked');
				if ($(this).is('[type="radio"]')) {
					$(this).siblings('[data-icon]').closest('.checkbox').siblings('.checkbox').removeClass('checked').children('[type="radio"]').prop('checked', false).siblings('[data-icon]').attr('data-prefix', 'far');
				}
			} else {
				$(this).siblings('[data-icon]').attr('data-prefix', 'far').closest('.checkbox').removeClass('checked');
			}
		}
	});
	$('input[type="checkbox"], input[type="radio"]').on('change', function() {
		if ($(this).closest('[class*="list-"]').is('.list-standard')) {
			if ($(this).is(':checked')) {
				$(this).siblings('[data-icon]').attr('data-prefix', 'fas').closest('.checkbox').addClass('checked');
				if ($(this).is('[type="checkbox"]')) {
					$(this).siblings('[data-icon]').attr('data-icon', 'check-square');
				} else if ($(this).is('[type="radio"]')) {
					$(this).siblings('[data-icon]').attr('data-icon', 'dot-circle').closest('.checkbox').siblings('.checkbox').removeClass('checked').children('[type="radio"]').prop('checked', false).siblings('[data-icon]').attr({'data-icon': 'circle', 'data-prefix': 'far'});
				}
			} else {
				$(this).siblings('[data-icon]').attr('data-prefix', 'far').closest('.checkbox').removeClass('checked');
				if ($(this).is('[type="checkbox"]')) {
					$(this).siblings('[data-icon]').attr('data-icon', 'square');
				} else if ($(this).is('[type="radio"]')) {
					$(this).siblings('[data-icon]').attr('data-icon', 'circle');
				}
			}
		} else if ($(this).closest('[class*="list-"]').is('.list-custom')) {
			if ($(this).is(':checked')) {
				$(this).siblings('[data-icon]').attr('data-prefix', 'fas').closest('.checkbox').addClass('checked');
				if ($(this).is('[type="radio"]')) {
					$(this).siblings('[data-icon]').closest('.checkbox').siblings('.checkbox').removeClass('checked').children('[type="radio"]').prop('checked', false).siblings('[data-icon]').attr('data-prefix', 'far');
				}
			} else {
				$(this).siblings('[data-icon]').attr('data-prefix', 'far').closest('.checkbox').removeClass('checked');
			}
		}
	});
	// open & close modal
	$('#open-modal, #open-modal-top').on('click', function(e) {
		e.preventDefault();
		$('aside').fadeIn(200);
	});
	$('#close-modal').on('click', function(e) {
		e.preventDefault();
		$('aside').fadeOut(200);
	});
	// generating code blocks for walkthrough modal
	$('pre code').each(function(i, block) { hljs.highlightBlock(block); });
});