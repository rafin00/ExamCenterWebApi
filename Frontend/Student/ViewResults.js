$(function(){
	
	if(localStorage.getItem('usertype')==null||localStorage.getItem('usertype')==""||localStorage.getItem('usertype')!='Student')
	{
		window.location.href = "../index.html";
	}
	var username = localStorage.getItem('username')
	
	
	var load = function(){
		
		
		var send = localStorage.getItem("username");
		console.log(send);
		$.ajax({
        async: false,
		headers: {
    "Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))
  },
        url: 'http://localhost:60270/api/students/'+localStorage.getItem("username")+'/evnts/finished',
		complete: function(xmlhttp,status){
				if(xmlhttp.status == 200)
				{	var result=xmlhttp.responseJSON;
			console.log(result.length); 
			var output="<table><tr><th>Event Name</th><th>Marks Obtained</th><th>Total Marks</th><th>Percantage</th><td></td></tr>";
					for(var i=0;i<result.length;i++)
					{	console.log(result[i]);
			var	nameoutput=result[i].EvntName;
						var res=0;
					//getresult
						$.ajax({
        async: false,
		headers: {
    "Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))
  },
        url: 'http://localhost:60270/api/students/'+localStorage.getItem("username")+'/registrations/'+result[i].EvntId,
		complete: function(xmlhttp,status){
				if(xmlhttp.status == 200)
				{	var regresult=xmlhttp.responseJSON;
			console.log(regresult); 
			
					//for(var i=0;i<result.length;i++)
					{
						
						
						res=regresult.Result;
						
						}
					//console.log(xmlhttp.responseJSON);
					
					
				}
				else
				{
					$('#results').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
		}
    });
						
						//name edn
						var totalqe=0;
						//totalques
						$.ajax({
        async: false,
		headers: {
    "Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))
  },
        url: 'http://localhost:60270/api/students/'+localStorage.getItem("username")+'/evnts/'+result[i].EvntId+'/questions',
		complete: function(xmlhttp,status){
				if(xmlhttp.status == 200)
				{	var totalresult=xmlhttp.responseJSON;
			
					
					//for(var i=0;i<result.length;i++)
					
						
						
						
						totalqe=totalresult.length;
						
					//console.log(xmlhttp.responseJSON);
					
					
				}
				else
				{
					$('#results').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
		}
    });
						//totalquesend
						
						
						output+='<tr align=center><td>'+nameoutput+'</td><td>'+res+'</td><td>'+totalqe+'</td><td>'+(res/totalqe)*100+'</td><td><a href="ViewEventResultScript.html?EvntId='+result[i].EvntId+'">View Script</a></td></tr>';
						}
					//console.log(xmlhttp.responseJSON);
					$('#body').html(output+'</table>');
					
				}
				else
				{
					$('#results').html(xmlhttp.status + ": " + xmlhttp.statusText);
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