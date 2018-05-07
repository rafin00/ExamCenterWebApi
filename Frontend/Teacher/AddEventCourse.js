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
	
	var coursetot=0;
	
	var coursetotalques = function(){
		
		$.ajax({
			url: 'http://localhost:60270/api/teachers/'+username+'/courses/'+ $('#courses option:selected').val()+'/questions',
			async: false,
			headers: {"Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))},
			complete: function(xmlhttp,status){
				if(xmlhttp.status==200)
				{   output='<table border=1><tr><th colspan=3>'+$('#courses option:selected').val()+'</th></tr>';
					var result=xmlhttp.responseJSON;
					$('#crstotal').html(result.length);
					coursetot=result.length;
						
					
						
					
				
				
				}
				
			}
		});//
	}
	
	var load = function(){
		var EvntName="";
		
		//evnt name
		$.ajax({
			type: "GET",
			url: 'http://localhost:60270/api/teachers/'+username+'/evnts/'+getUrlParameter('EvntId'),
			 async: false,
		headers: {
    "Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))
  },
			complete: function(xmlhttp,status){
				if(xmlhttp.status==200)
				{
					var result=xmlhttp.responseJSON;
					var output='<table><tr align="center"><th colspan=2>'+result.EvntName+'</th></tr>';
					//getcourses
					$.ajax({
			type: "GET",
			url: 'http://localhost:60270/api/teachers/'+username+'/courses/',
			 async: false,
		headers: {
    "Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))
  },
			complete: function(xmlhttp,status){
				if(xmlhttp.status==200)
				{
					var result=xmlhttp.responseJSON;
					output+='<tr><td><select id="courses">'; 
					for(var i=0;i<result.length;i++)
					{
						output+='<option value="'+result[i].CourseName+'">'+result[i].CourseName+'</option>';  
					}
					output+='</select><td>Total Questions:<label id="crstotal"></label></td><tr>';
					output+='<tr><td><input name="method" value=1 type="radio">Uploal From File</td><td><input name="method" value=2 type="radio">Select Manually</td></tr></table>'
				$('#msg').html(output);
					coursetotalques();
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
					//getcourses end
					
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
	var questable = function(){
		
		var output="";
					//courses question
					$.ajax({
			url: 'http://localhost:60270/api/teachers/'+username+'/courses/'+ $('#courses option:selected').val()+'/questions',
			async: false,
			headers: {"Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))},
			complete: function(xmlhttp,status){
				if(xmlhttp.status==200)
				{   output='<table border=1><tr><th colspan=3>'+$('#courses option:selected').val()+'</th></tr>';
					var result=xmlhttp.responseJSON;
					for(var i=0;i<result.length;i++)
					{
						
						//check checked
						var chkd="";
						$.ajax({
			url: 'http://localhost:60270/api/teachers/'+username+'/evnts/'+getUrlParameter('EvntId')+'/courses/'+$('#courses option:selected').val()+'/questions/'+result[i].QuestionId,
			async: false,
			headers: {"Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))},
			complete: function(xmlhttp,status){
				if(xmlhttp.status==200)
				{
					chkd="checked"
				}
				else
				{
					chkd="";
				}
			}
		});
						//console.log(result[i].QuestionId+' '+chkd);
						//
						output+='<tr><td><input onclick="myfuc(this)" name="change" '+chkd+' type="checkbox" value='+result[i].QuestionId+'></td><td>Question no. '+(i+1)+'</td><td>'+result[i].QuestionText+'</td></tr>';
						output+='<tr><td></td><td>Answer : </td><td>'+result[i].Answer+'</td></tr>';chkd="";
					//question options
						$.ajax({
			url: 'http://localhost:60270/api/teachers/'+username+'/options/'+result[i].QuestionId,
			async: false,
			headers: {"Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))},
			complete: function(xmlhttp,status){
				if(xmlhttp.status==200)
				{
					var optionresult=xmlhttp.responseJSON;
					for(var j=0;j<optionresult.length;j++)
					{
						output+='<tr><td></td><td>Option No. '+(j+1)+'</td><td>'+optionresult[j].OptionText+'</td></tr>';
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
						
					
						$('#body').html(output+'</table>');
					
				
				
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
		});//
					
			
	}
	
	$('input[type="checkbox"]').change(function() {
	console.log($(this).val());
	console.log('fsds');
	});
	
	var randtable = function(){
	
		var	output='<table><tr><th colspan=2>Select File: </th></tr><tr><td><input type="file" id="file1" ></td><td><button id="btnPostFile">Upload</button></td></tr><tr><td colspan=2><label id="rmsg"></label></td></tr></table>'
		$('#body').html(output);
		upload();
	}
var upload = function(){
	$('#btnPostFile').click(function () {
            if ($('#file1').val() == '') {
                alert('Please select file');
                return;
            }
 
            var formData = new FormData();
            var file = $('#file1')[0];
            formData.append('file', file.files[0]);
            $.ajax({
				type: "POST",
                url: 'http://localhost:60270/api/teachers/fileUpload/evnts/'+getUrlParameter('EvntId')+'/courses/'+$('#courses option:selected').val(),
                data: formData,
				headers: {"Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))},
			
                contentType: false,
                processData: false,
                success: function (d) {
					
                    $('#rmsg').html(d);
                    $('#file1').val(null);
                },
                error: function () {
                   
                    $('#rmsg').html('Failed Syntax error');
					}
            });
        });
}
	
	$('input[type=radio]').on('change', function() {
	if($(this).val()==2)
	{		questable();
	}
	else
	{
		randtable();
	}
	});
	
	$('.logout').click(function(){
	localStorage.setItem("username",'');
					localStorage.setItem("password",'');
					window.location.href = "../index.html";
	});

	
	
	
	 $("#courses").change(function () {
		 coursetotalques();
	 if($('input[name=method]:checked').val()==1)
	 {		
		 
	 }
	 else if($('input[name=method]:checked').val()==2)
	 {	
		 questable();
	 }
		 
	 
	 });
	
	
});

function myfuc(obj)
{ 
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

var	exam = {EvntId:getUrlParameter('EvntId'),QuestionId:$(obj).val(),CourseName:$('#courses option:selected').val()};
	console.log($(obj).val())
	$.ajax({
        type: "POST",
        data :JSON.stringify(exam),
			url: 'http://localhost:60270/api/teachers/'+localStorage.getItem("username") +'/evnts/'+getUrlParameter('EvntId')+'/courses/'+$('#courses option:selected').val()+'/questions',
			 async: false,
			 contentType: "application/json",
			headers: {
    "Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))
  },
			complete: function(xmlhttp,status){
				if(xmlhttp.status==200)
				{
					console.log('added');
					
					
					
				}
				else 
				{
					console.log('No content');
					
				}
			}
		});
}