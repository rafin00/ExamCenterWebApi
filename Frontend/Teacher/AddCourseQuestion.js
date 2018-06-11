$(function(){
	
	if(localStorage.getItem('usertype')==null||localStorage.getItem('usertype')==""||localStorage.getItem('usertype')!='Teacher')
	{
		window.location.href = "../index.html";
	}
	var username = localStorage.getItem('username');
	var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : sParameterName[1];
        }
    }
	};
	
	
	
	var upload = function(){
	$('#uploadbtn').click(function () {
            if ($('#file1').val() == '') {
                alert('Please select file');
                return;
            }
 
            var formData = new FormData();
            var file = $('#file1')[0];
            formData.append('file', file.files[0]);
            $.ajax({
				type: "POST",
                url: 'http://localhost:60270/api/teachers/fileUpload/courses/'+getUrlParameter('CourseName'),
                data: formData,
				headers: {"Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))},
			
                contentType: false,
                processData: false,
                success: function (d) {
					
                    $('#msg').html(d);
                    $('#file1').val(null);
                },
                error: function () {
                   
                    $('#msg').html('Failed Syntax error');
					}
            });
        });
}	

	var manual = function(){
		
	
	}


	var file = function(){
		var output='<table border=0><tr ><th colspan=2>New Question</th></tr><tr><td><input type="file" accept="text/plain" id="file1"></td><td><button id="uploadbtn">Add</button></td></tr></table>';
		 $('#body1').html(output);
		 upload();
	}
	
	var load = function(){
		var EvntName="";
		
			
		
		file();
	
	}
	load();
	$('.logout').click(function(){
	localStorage.setItem("username",'');
					localStorage.setItem("password",'');
					window.location.href = "../index.html";
	});
	

	
	
	
	
	
});