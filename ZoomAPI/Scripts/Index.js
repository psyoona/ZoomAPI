$(document).ready(function () {
	$('#create').click(function (e) {
		$.ajax({
			url: 'Home/CreateZoomMeeting',
			type: 'POST',
			success: function (response) {
				if (response) {
					var parsed = JSON.parse(response);

					$('#host').val(parsed.StartUrl);
					$('#join').val(parsed.JoinUrl);
					$('#code').val(parsed.Code);
				}
				console.log('Success');
			},
			error: function (jqXHR, textStatus, errorThrown) {
				console.log(jqXHR);
				console.log(textStatus);
				console.log(errorThrown);
			}
		});
	});

	console.log('hihi');
});