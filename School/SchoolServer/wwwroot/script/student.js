async function createItem(params) {
    let elem = document.createElement("a")
    elem.role = "button"
    elem.href = `${urlApplicate}/viewclass.html?id=${params.id}&number=${params.number}&letter=${params.letter}`
    elem.style.textDecoration = "none"
    elem.text = params.number + params.letter
    return elem
}

async function fetchSubjects() {
    const url = `${urlApplicate}/api/Subject`
    const response = await fetch(
        url,
        {
            method: 'GET',
        }
    )
    const data = await response.json()
    return data
}

// Функция для отображения информации
async function displayInfo(info) {
    // Создаем контейнер для информации
    const infoContainer = document.createElement('div');
    infoContainer.classList.add('info-container');
    infoContainer.classList.add('container');

    // Заполнение информации о студенте
    const studentInfo = document.createElement('div');
    studentInfo.innerHTML = `
        <h2>Данные ученика</h2>
        <p><strong>ФИО:</strong> ${info.student.firstName} ${info.student.lastName} ${info.student.patronymic}</p>
        <p><strong>ID:</strong> ${info.student.id}</p>
        <p><strong>Паспорт:</strong> ${info.student.passport}</p>
        <p><strong>Дата рождения:</strong> ${new Date(info.student.birthDate).toLocaleDateString()}</p>
    `;
    infoContainer.appendChild(studentInfo);

    // Заполнение информации о классе
    const classInfo = document.createElement('div');
    classInfo.innerHTML = `
        <h3>Информация о классе</h3>
        <p><strong>Класс:</strong> ${info.class.number} ${info.class.letter}</p>
    `;
    infoContainer.appendChild(classInfo);

    // Создание таблицы для оценок
    const gradesTable = document.createElement('table');
    gradesTable.classList.add('grade-table');

    const thead = document.createElement('thead');
    thead.innerHTML = `
        <tr>
            <th>Предмет</th>
            <th>Оценка</th>
            <th>Дата</th>
        </tr>
    `;
    gradesTable.appendChild(thead);

    let subjects = await fetchSubjects()

    const tbody = document.createElement('tbody');
    info.grades.forEach(grade => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${subjects.find(s => s.id == grade.subjectId).name}</td>
            <td>${grade.mark}</td>
            <td>${new Date(grade.date).toLocaleDateString()}</td>
        `;
        tbody.appendChild(row);
    });
    gradesTable.appendChild(tbody);
    infoContainer.appendChild(gradesTable);

    // Добавляем контейнер с информацией на страницу
    document.body.appendChild(infoContainer);
}

(async () => {
    let url = window.location.href.slice();
    try {
        const response = await fetch(
            `${urlApplicate}/api/Student/GetUserID`,
            {
                method: 'GET'
            }
        )
        const userId = await response.json()

        const studentInfo = await fetch(
            `${urlApplicate}/api/Student/GetStudentByUserID`,
            {
                method: 'POST',
                body: userId,
                headers: { 'Content-Type': 'application/json' }
            }
        )
        const info = await studentInfo.json();

        await displayInfo(info);

    } catch (error) {
        console.error(error.message)
    }

})()