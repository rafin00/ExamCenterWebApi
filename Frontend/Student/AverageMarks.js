$(function(){
	
	if(localStorage.getItem('usertype')==null||localStorage.getItem('usertype')==""||localStorage.getItem('usertype')!='Student')
	{
		window.location.href = "../index.html";
	}
	var username = localStorage.getItem('username')
	var load = function(){
		var dt = new Date();
		var time = dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
		var date = dt.getYear()+'-'+dt.getMonth()+'-'+dt.getDate();
		var datetime=new Date(date)+' '+time;
		
		$.ajax({
			url: 'http://localhost:60270/api/students/'+username+'/courses/average',
			 async: false,
		headers: {
    "Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))
  },
			complete: function(xmlhttp,status){
				if(xmlhttp.status==200)
				{
					var result=xmlhttp.responseJSON;
					var output='<table border=1 cellspacing=0 cellpadding=0><tr><th>Course Name</th><th>Average Marks</th></tr>';
					for(var i=0;i<result.length;i++)
					{	
						
						output +='<tr><td>'+result[i].CourseName+'</td><td>'+result[i].Mark+'</td></tr>';
						
					}
					output+='</table>'
					$('#body').html(output);
					
					
					
				}
				else if(xmlhttp.status==404)
				{
					$('#body').html("</br>No records.");
				}
				else
				{$('#body').html(xmlhttp.status + ": " + xmlhttp.statusText);
					//$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
	
	}
	load();
	$('.logout').click(function(){
	localStorage.setItem("username",'');
					localStorage.setItem("password",'');
					localStorage.setItem("usertype",'');
					window.location.href = "../index.html";
	});
	
	//delete evnt btn
	$( ".deleteevnt" ).click(function(){
		console.log($(this).val());
		var answer = confirm('Are you sure you want to delete this event?'+$(this).val());
		if (answer)
		{
			//delete ques
			$.ajax({
				 type: "Delete",
			url: 'http://localhost:60270/api/teachers/'+username+'/evnts/'+$(this).val(),
			 async: false,
		headers: {
    "Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))
  },
			complete: function(xmlhttp,status){
				if(xmlhttp.status==204)
				{
					location.reload();
					
				}
				else
				{$('#body').html(xmlhttp.status + ": " + xmlhttp.statusText);
					//$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
			//delete ques end
			console.log($(this).val());
		}
		else
		{
			console.log('cancel');
		}
	});
	//delete evnt btn end
});