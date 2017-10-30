$("#selProyectosTareasFilter").on('change', validateTasks);

function validateTasks() {
	var proyectId = this.value;
	var tareasDropdown = $('#selTareaFilter');
	tareasDropdown.children().remove();

	if (proyectId) {
		var $url = base_url_gestion + "Tareas/GetTareasByProyectoId/" + proyectId;
		$.ajax({
			url: $url,
			type: 'GET',
			success: function (response) {
				var tasks = JSON.parse(response);
				$.each(tasks, function (index, task) {
					tareasDropdown.append($('<option>').text(task.Descripcion).attr('value', task.Id));
				});
			},
			error: function (error) {
				console.log(error.responseText);
			}
		});
	}

}