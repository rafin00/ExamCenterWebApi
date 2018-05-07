$(function(){
	
	if(localStorage.getItem('usertype')==null||localStorage.getItem('usertype')==""||localStorage.getItem('usertype')!='Student')
	{
		window.location.href = "../index.html";
	}
	var username = localStorage.getItem('username')
	
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
	
	var submitfunc=function()
	{
		$( "#submit" ).click(function(){
		
		$.ajax({
        type: "POST",
        data :JSON.stringify(),
			url: 'http://localhost:60270/api/students/'+localStorage.getItem("username")+'/evnts/'+getUrlParameter('EvntId'),
			 async: false,
			 contentType: "application/json",
			headers: {
    "Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))
  },
			complete: function(xmlhttp,status){
				if(xmlhttp.status==200)
				{
					window.location.href = "StudentHome.html";
					
					
					
				}
				else if(xmlhttp.status==409)
				{
					$('#msg').html("UserName Already Exists");
				}else 
				{$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
					//$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
		
		});
	}
	
	
	var changefunc= function(){
			
		$('input[type=radio][class=questions]').on('change', function() {	
	console.log($(this).val());
			var questionid =$(this).val().split(',')[0];
			var myanswer =$(this).val().split(',')[1];
			 var answer = {EvntId : getUrlParameter('EvntId'),QuestionId:questionid,MyAnswer:myanswer,StudentUserName:localStorage.getItem("username")};
  console.log(answer);
			$.ajax({
        type: "PUT",
        data :JSON.stringify(answer),
			url: 'http://localhost:60270/api/students/'+localStorage.getItem("username")+'/evnts/'+getUrlParameter('EvntId')+'/questions/'+questionid+'/answers',
			 async: false,
			 contentType: "application/json",
			headers: {
    "Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))
  },
			complete: function(xmlhttp,status){
				if(xmlhttp.status==200)
				{
					$('#msg').html("");
					
					
					
				}
				else if(xmlhttp.status==409)
				{
					$('#msg').html("UserName Already Exists");
				}else 
				{$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
					//$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
			
		});
	}
	
	var getQuestions = function(){
		console.log($('input[name=course1]:checked').val());
	$.ajax({
			url: 'http://localhost:60270/api/students/'+username+'/evnts/'+getUrlParameter('EvntId')+'/courses/'+$('input[name=course1]:checked').val()+'/questions',
			
			async: false,
			headers: {"Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))},
			complete: function(xmlhttp,status){
				if(xmlhttp.status==200)
				{
					var result=xmlhttp.responseJSON;
					var output1='<table border=1>';
					for(var i=0;i<result.length;i++)
					{
						output1+='<tr><td><pre> Question no. '+(i+1)+' </pre></td><td><pre> '+result[i].QuestionText+'</pre></td><tr>';
						
							//ckh
							 var answer = {EvntId : getUrlParameter('EvntId'),QuestionId:result[i].QuestionId,MyAnswer:"sdf",StudentUserName:localStorage.getItem("username")};
  console.log(answer); var ans="";
			$.ajax({
        type: "GET",
        
			url: 'http://localhost:60270/api/students/'+localStorage.getItem("username")+'/evnts/'+getUrlParameter('EvntId')+'/questions/'+result[i].QuestionId+'/answers',
			 async: false,
			 contentType: "application/json",
			headers: {
    "Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))
  },
			complete: function(xmlhttp,status){
				if(xmlhttp.status==200)
				{
					ans=xmlhttp.responseJSON.MyAnswer;
					
					console.log(ans);
					
				}
				else if(xmlhttp.status==409)
				{
					$('#msg').html("UserName Already Exists");
				}else 
				{$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
					//$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
							//ckh end
						
						//get options
						
						$.ajax({
			url: 'http://localhost:60270/api/students/'+username+'/evnts/'+getUrlParameter('EvntId')+'/courses/'+getUrlParameter('CourseName')+'/questions/'+result[i].QuestionId+'/options',
			async: false,
			headers: {"Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))},
			complete: function(xmlhttp,status){
				if(xmlhttp.status==200)
				{var cchk="";
					var optionresult=xmlhttp.responseJSON;
					for(var j=0;j<optionresult.length;j++)
					{
						
						console.log(ans+' '+optionresult[j].OptionText);
						if(ans==optionresult[j].OptionText)
						{
						output1+='<tr><td><input checked="checked" type="radio" class="questions" name='+result[i].QuestionId+' value="'+result[i].QuestionId+','+optionresult[j].OptionText+'" ></td><td>'+optionresult[j].OptionText+'</td></tr>';
							
						}
						else
						{output1+='<tr><td><input  type="radio" class="questions" name='+result[i].QuestionId+' value="'+result[i].QuestionId+','+optionresult[j].OptionText+'" ></td><td>'+optionresult[j].OptionText+'</td></tr>';
						}
					}
				}
				else
				{$('#body1').html(xmlhttp.status + ": " + xmlhttp.statusText);
					//$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
						
						//get options end
							
					}
						output1+='<tr  align=center><td colspan=2 ><button id="submit" >Finish</button></td></tr></table>'
					//console.log(output);
						$('#body1').html(output1);
						console.log(output1+'</table>');
					changefunc();
					submitfunc();
				
				}
				else if(xmlhttp.status==204)
				{
					$('#body1').html("</br>No Such Events.");
				}
				else
				{$('#body1').html(xmlhttp.status + ": " + xmlhttp.statusText);
					//$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
	
	}
	
	var output="";
	var getcourses = function(){
		$.ajax({
			url: 'http://localhost:60270/api/students/'+username+'/evnts/'+getUrlParameter('EvntId')+'/courses',
			 async: false,
		headers: {
    "Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))
  },
			complete: function(xmlhttp,status){
				if(xmlhttp.status==200)
				{
					var result=xmlhttp.responseJSON;
					output+='<tr><td>Courses : </td><td>'+result.length+'</td></tr>';
					var sel='checked="checked"';
					for(var i=0;i<result.length;i++)
					{	
						output+='<tr><td></td><td><input name="course1" type="radio" '+sel+' value="'+result[i].CourseName+'">'+result[i].CourseName+'</td></tr>'
					sel="";
					
					}
					
					
					
					$('#body').html(output+'</table>');
					
					getQuestions();
					
				}
				else if(xmlhttp.status==204)
				{
					$('#body').html("</br>No Events.");
				}
				else
				{$('#body').html(xmlhttp.status + ": " + xmlhttp.statusText);
					//$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
	
	}
	
	var load = function(){
		var dt = new Date();
		var time = dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
		var date = dt.getYear()+'-'+dt.getMonth()+'-'+dt.getDate();
		var datetime=new Date(date)+' '+time;
		
		$.ajax({
			url: 'http://localhost:60270/api/students/'+username+'/evnts/'+getUrlParameter('EvntId'),
			 async: false,
		headers: {
    "Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))
  },
			complete: function(xmlhttp,status){
				if(xmlhttp.status==200)
				{
					var result=xmlhttp.responseJSON;
					output='<table border=1 cellspacing=0 cellpadding=0><tr><th>Event Name</th><th>'+result.EvntName+'</th></tr>';
					output +=	'<tr><th>Start Time</th><th>'+result.EvntSdt+'</th></tr>';
					output += '<tr><th>End Time</th><th>'+result.EvntSdt+'</th></tr>';
					
					
					getcourses();
					
				}
				else if(xmlhttp.status==204)
				{
					$('#body').html("</br>UnAuthorized");
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
	
	$('input[type=radio][name=course1]').on('change', function() {	
	getQuestions();
	});
});