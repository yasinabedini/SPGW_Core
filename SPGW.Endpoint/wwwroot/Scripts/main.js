$(()=>{
  if($('col-brand > brand').length < 4){
    $('container > column > col-body col-brand').css('grid-template-columns', `repeat(${$('col-brand > brand').length}, 1fr)`)
  }
})