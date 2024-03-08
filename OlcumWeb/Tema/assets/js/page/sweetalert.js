"use strict";

$("#swal-1").click(function () {
  swal('Merhaba');
});

$("#swal-2").click(function () {
  swal('Good Job', 'You clicked the button!', 'success');
});

$("#swal-3").click(function () {
  swal('Good Job', 'You clicked the button!', 'warning');
});

$("#swal-4").click(function () {
  swal('Good Job', 'You clicked the button!', 'info');
});

$("#swal-5").click(function () {
  swal('Good Job', 'You clicked the button!', 'error');
});

$("#swal-6").click(function () {
  swal({
    title: 'Emin misiniz?',
    text: 'Sildiðiniz zaman, bu dosyanýn geri dönüþü olmayacaktýr!',
    icon: 'warning',
    buttons: true,
    dangerMode: true,
  })
    .then((willDelete) => {
      if (willDelete) {
        swal('Gitti! Silme iþleminiz yapýldý!', {
          icon: 'success',
        });
      } else {
        swal('Dosyanýz güvende!');
      }
    });
});

$("#swal-7").click(function () {
  swal({
    title: 'What is your name?',
    content: {
      element: 'input',
      attributes: {
        placeholder: 'Type your name',
        type: 'text',
      },
    },
  }).then((data) => {
    swal('Hello, ' + data + '!');
  });
});

$("#swal-8").click(function () {
  swal('This modal will disappear soon!', {
    buttons: false,
    timer: 3000,
  });
});