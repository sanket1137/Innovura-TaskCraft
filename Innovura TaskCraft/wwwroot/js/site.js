var labelColors = {};
var taskUpdated = {};

$(document).ready(function () {

    //on page load get labels
    $.get('/api/tasks/labels', function (labels) {
        //var labelDropdown = $('#myLabels');
        $('#myLabels').empty();
        $.each(labels, function (index, label) {
            $('#myLabels').append('<option class="' + label.id + '" value="' + label.labelName + '"></option>');
            labelColors[label.id] = label.color;
        });
    });




    //delete Task click
    $(document).on('click', '.deleteTask', function () {
        var taskName = $(this).data('task-name');
        var taskId = $(this).data('task-id');

        $('#deleteTaskTitle').text(taskName);
        $('#deleteTask').text(taskId);
    });

    //Update Task
    $('#UpdateTaskButton').on('click', function () {
        var taskName = $(this).data('task-name');
        var taskId = $(this).data('task-id');
        //$('#updateLabels').val(data.label.labelName);
        var task = {
            Id: taskId,
            taskName: $('#updatetaskName').val(),
            state: $('#updatestate').val(),
            createdDate: $('#updatecreatedDate').val(),
            startedDate: $('#updatestartedDate').val(),
            endDate: $('#updateendDate').val(),
            labelId: $('#updatemyLabels option[value="' + $('#updateLabels').val() + '"]').attr('class')
        };
        
        if (taskUpdated.taskName !== task.taskName || taskUpdated.state !== task.state || taskUpdated.startedDate !== task.startedDate || taskUpdated.endDate !== task.endDate || taskUpdated.labelName !== task.labelName) {
            
            // Make AJAX request to create task
            $.ajax({
                url: '/api/tasks/taskupdate',
                method: 'PATCH',
                contentType: 'application/json',
                data: JSON.stringify(task),
                success: function () {

                    $('#updateTaskModal').modal('hide');
                    refreshTable();
                },
                error: function (xhr, textStatus, errorThrown) {
                    // Handle error
                    console.error('Failed to create task.', textStatus, errorThrown);
                }
            });
        } 
        
    });


    //create Task click
    $('#createTaskButton').on('click', function () {

        var input = $('#Labels').val();
        var createdLabelId = 0;
        var datalistOptions = $('#myLabels').find('option').map(function () {
            return $(this).val();
        }).get();

        var isInOptions = datalistOptions.includes(input);
        var isNewLabelVisible = $('#newLabel').is(':visible');
        if (!isInOptions && !isNewLabelVisible) {
            $('#newLabel').show();
            $('#timeHours').focus();
            return;
        } else if (!isInOptions) {
            var timeHours = $('#timeHours').val();
            var labelColor = $('#exampleColorInput').val();
            var label = {
                labelName: input,
                color: labelColor
                //timeSpan: timeHours
            };
         

            $.ajax({
                url: '/api/tasks/createlabel',
                method: 'POST',
                async: false,
                contentType: 'application/json',
                data: JSON.stringify(label),
                success: function (createdLabel) {

                    createdLabelId = createdLabel;
                },
               
            });
        } else {
            createdLabelId = $('#myLabels option[value="' + input + '"]').attr('class');
        }

        var taskData = {
            taskName: $('#taskName').val(),
            state: $('#state').val(),
            /*createdDate: $('#createdDate').val(),*/
            startedDate: $('#startedDate').val(),
            endDate: $('#endDate').val(),
            labelId: createdLabelId
        };

        // Make AJAX request to create task
        $.ajax({
            url: '/api/tasks/taskcreate',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(taskData),
            success: function () {
                
                $('#createTaskModal').modal('hide');
                refreshTable();
            },
            error: function (xhr, textStatus, errorThrown) {
                // Handle error
                console.error('Failed to create task.', textStatus, errorThrown);
            }
        });
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
function formatDate(date) {
    // Format date as yyyy-MM-dd
    if (date) {
        const formattedDate = new Date(date).toISOString().split('T')[0];
        return formattedDate;
    }
    return '';
}
    function updateTask(id) {
        $.ajax({
            url: '/api/tasks/'+ id,
            method: 'GET',
            contentType: 'application/json',

            success: function (data) {
                $('#updatetaskName').val(data.taskName);
                $('#updatestate').val(data.state);
                $('#updatecreatedDate').val(formatDate(data.createdDate));
                $('#updatestartedDate').val(formatDate(data.startedDate));
                $('#updateendDate').val(formatDate(data.endDate));
                
                $('#updatemyLabels').empty();
                
                getLabels();
                $('#updateLabels').val(data.label.labelName);

                taskUpdated = {
                    taskName: data.taskName,
                    state: data.state,
                    startedDate: data.startedDate,
                    endDate: data.endDate,
                    labelName: data.label.labelName
                }
                $('#UpdateTaskButton').data('task-id', id);
            },
            error: function (xhr, textStatus, errorThrown) {
                console.error('Failed to retrieve tasks.', textStatus, errorThrown);
            }
        });
    }
function getLabels() {
    return new Promise(function (resolve, reject) {
        $.get('/api/tasks/labels', function (labels) {
            $('#updatemyLabels').empty();
            $.each(labels, function (index, label) {
                $('#updatemyLabels').append('<option class="' + label.id + '" value="' + label.labelName + '"></option>');
               
            });

            resolve(); 
        }).fail(function (error) {
            reject(error);  
        });
    });
}

// Usage
getLabels().then(function () {
    // Handle success or further processing
}).catch(function (error) {
    console.error('Failed to load labels.', error);
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
                                 <button type="button" class="updateTask btn btn-success" data-bs-toggle="modal" data-bs-target="#updateTask"  data-task-uname="${task.taskName}" data-task-uid="${task.id}" onclick="updateTask(${task.id})"><i class="bi bi-pencil-square"></i></button>
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