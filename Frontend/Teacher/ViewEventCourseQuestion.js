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
	
	
	var load = function(){
		var EvntName="";
		
		//evnt name
		$.ajax({
			url: 'http://localhost:60270/api/teachers/'+username+'/evnts/'+getUrlParameter('EvntId'),
			 async: false,
		headers: {
    "Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))
  },
			complete: function(xmlhttp,status){
				if(xmlhttp.status==200)
				{
					var result=xmlhttp.responseJSON;
					//var output="<table border=1 cellspacing=0 cellpadding=0><tr><th>Event Name</th><th>Start Time</th><th>End Time</th><th></th><th></th></tr>";
					//for(var i=0;i<result.length;i++)
						
						
					var	EvntName =result.EvntName;
						
					//evnt courses question
					$.ajax({
			url: 'http://localhost:60270/api/teachers/'+username+'/evnts/'+getUrlParameter('EvntId')+'/courses/'+getUrlParameter('CourseName')+'/questions',
			async: false,
			headers: {"Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))},
			complete: function(xmlhttp,status){
				if(xmlhttp.status==200)
				{
					var result=xmlhttp.responseJSON;
					var output='<div>'+EvntName+'</div><div>'+getUrlParameter('CourseName')+'</div><div>Questions</div>';
					for(var i=0;i<result.length;i++)
					{
						output+='<Button class="question" value='+result[i].QuestionId+' >Remove</Button><div><pre> Question no. '+(i+1)+' '+result[i].QuestionText+'</pre></div><div><pre>Answer: '+result[i].Answer+'</pre></div>';
						//get options
						
						$.ajax({
			url: 'http://localhost:60270/api/teachers/'+username+'/evnts/'+getUrlParameter('EvntId')+'/courses/'+getUrlParameter('CourseName')+'/questions/'+result[i].QuestionId+'/options',
			async: false,
			headers: {"Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))},
			complete: function(xmlhttp,status){
				if(xmlhttp.status==200)
				{
					var optionresult=xmlhttp.responseJSON;
					for(var j=0;j<optionresult.length;j++)
					{
						output+='<div><pre>Option no. '+(j+1)+' '+optionresult[j].OptionText+'</pre></div>';
						
					}
				}
				else
				{$('#body').html(xmlhttp.status + ": " + xmlhttp.statusText);
					//$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
						
						//get options end
							
					}
						
					
						$('#body').html(output);
					
				
				
				}
				else if(xmlhttp.status==204)
				{
					$('#body').html("</br>No Such Events.");
				}
				else
				{$('#body').html(xmlhttp.status + ": " + xmlhttp.statusText);
					//$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
					
					//evnt courses end
				
				
				}
				else if(xmlhttp.status==204)
				{
					$('#body').html("</br>No Such Events.");
				}
				else
				{$('#body').html(xmlhttp.status + ": " + xmlhttp.statusText);
					//$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
	
	}
	load();
	
	$( ".question" ).click(function(){
		
		var answer = confirm('Are you sure you want to delete this question?');
		if (answer)
		{
			//delete ques
			$.ajax({
				 type: "Delete",
			url: 'http://localhost:60270/api/teachers/'+username+'/evnts/'+getUrlParameter('EvntId')+'/courses/'+getUrlParameter('CourseName')+'/questions/'+$(this).val(),
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
	});