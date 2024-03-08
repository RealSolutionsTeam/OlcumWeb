'use strict';

const input = document.getElementById('input');
const addBtn = document.getElementById('add');
const clearBtn = document.getElementById('clear');
const list = document.getElementById('list');

addBtn.addEventListener('click', createElement);

function createElement() {
	let value = input.value;
	if (value === '') {
		return;
	}
	input.value = ''; //learing the input field
	
	const item = document.createElement('li');
	item.className = 'list__item';
	list.append(item);

	const circle = document.createElement('span');
	circle.className = 'list__circle';
	item.append(circle);

	const text = document.createElement('div');
	text.className = 'list__text';
	text.innerHTML = value;
	item.append(text);

	getSaved();
	deleteElement();
};

function deleteElement () {
	const listItems = document.querySelectorAll('.list__item');
	for (let element of listItems) {
		element.addEventListener('click', () => element.remove());
	}
};

//adding an element by pressing Enter
input.addEventListener("keypress", (keyPressed) => {
	const keyEnter = 13;
	if (keyPressed.keyCode == keyEnter) {
		createElement();
	}
});

//deleting elements by button "Clear all"
clearBtn.addEventListener("click", () => {
	localStorage.removeItem('list', list.innerHTML = '');
})

// saving an item in local storage

function getSaved () {
	localStorage.setItem('list', list.innerHTML);
}

// getting data after page reloading
function loadTodos() {
	const data = localStorage.getItem('list');
	list.innerHTML = data;
	deleteElement();
};

//function call
loadTodos();