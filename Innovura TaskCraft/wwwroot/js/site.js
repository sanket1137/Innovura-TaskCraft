$(document).ready(function () {

    $(document).on('click', '.deleteTask', function () {
        var taskName = $(this).data('task-name');
        var taskId = $(this).data('task-id');

        $('#deleteTaskTitle').text(taskName);
        $('#deleteTask').text(taskId);
    });

    
    $('#confirmDelete').on('click', function () {
       
        var taskNameToDelete = $('#deleteTaskTitle').text();
        var taskIdToDelete = $('#deleteTask').text();
        //encodeURIComponent(taskIdToDelete)
        $.ajax({
            url: '/api/tasks/' + encodeURIComponent(taskIdToDelete),
            method: 'DELETE',
            contentType: 'application/json',
            success: function () {
                console.log('Task ' + taskNameToDelete + ' deleted successfully.');
                $('#deleteModal').find('.modal-body').html('<div class="alert alert-success">Done!</div>');
                $('#deleteModal').modal('hide');

                refreshTable();
            },
            error: function (xhr, textStatus, errorThrown) {
                console.error('Failed to delete task ' + taskNameToDelete + '.', textStatus, errorThrown);
            },
            complete: function () {
                $('#deleteModal').modal('hide');
                $('#deleteModal').find('.modal-body').html('Are you sure you want to delete the task with the title: <strong id="deleteTaskTitle"></strong>? <strong id="deleteTask" hidden></strong>');

            }
        });
    });




    // Function to update the table content
    
});



    function refreshTable() {
        
        $.ajax({
            url: '/api/tasks/tasks',
            method: 'GET',  
            contentType: 'application/json',

            success: function (data) {
                $("#taskTable").empty();

                if (data.length > 0) {
                    $.each(data, function (index, task) {
                        console.log('Processing task:', task);
                        var row = `<tr>
                             <td>${task.taskName}</td>
                             <td>${task.state}</td>
                             <td> <span style="background-color: ${task.label?.color ?? '#ffffff00'}">${task.label?.labelName ?? 'N/A'}</span></td>
                             <td>${task.createdDate}</td>
                             <td>${task.startedDate ?? "N/A"}</td>
                             <td>${task.endDate ?? "N/A"}</td>
                             <td>
                                 <button type="button" class="updateTask btn btn-success"><i class="bi bi-pencil-square"></i></button>
                                 <button type="button" class="deleteTask btn btn-danger" data-target="#deleteModal" data-toggle="modal"  data-task-name="${task.taskName}" data-task-id="${task.id}"><i class="bi bi-trash"></i></button>
                             </td>
                         </tr>`;
                        $("#taskTable").append(row);
                    });

                } else {
                    // Display a message if there are no tasks
                    var emptyRow = `<tr>
                                     <td colspan="7" style="text-align: center; font-weight: bold; color: gray;">No data to show</td>
                                 </tr>`;
                    $("#taskTable").append(emptyRow);
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                console.error('Failed to retrieve tasks.', textStatus, errorThrown);
            }
        });

    }