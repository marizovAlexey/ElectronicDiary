async function fetchSubjects() {
    const url = "http://localhost:5127/api/Subject"
    const response = await fetch(
        url,
        {
            method: 'GET',
        }
    )
    const data = await response.json()
    return data
}

async function fetchStudentsByClass(classId) {
    const url = "http://localhost:5127/api/Class/GetClassStudents"
    const response = await fetch(
        url,
        {
            method: 'POST',
            body: JSON.stringify(classId),
            headers: {'Content-Type': 'application/json'}
        }
    )
    const data = await response.json()
    return data
}

async function fetchGradesByStudent(studentId) {
    const url = "http://localhost:5127/api/Student/GetGrades"
    const response = await fetch(
        url,
        {
            method: 'POST',
            body: JSON.stringify(studentId),
            headers: {'Content-Type': 'application/json'}
        }
    )
    const data = await response.json()
    return data
}

async function createCell(content) {
    let cell = document.createElement("div")
    cell.innerText = content
    cell.style.minHeight = "30px"
    cell.style.border = "1px solid #0000ff"
    return cell
}

async function handleSubmit(gradeId, studentId, subjectId, grade) {
    const url = "http://localhost:5127/api/Grade"
    const data = {
        id: gradeId,
        subjectId: subjectId,
        studentId: studentId,
        mark: grade
    }
    console.log("handleSubmit", data)
    const response = await fetch(
        url,
        {
            method: 'POST',
            body: JSON.stringify(data),
            headers: {'Content-Type': 'application/json'}
        }
    )
    window.location.reload()
}

async function renderPage() {
    let classTitle = document.getElementById("classTitle")
    classTitle.parentElement.ariaBusy = true
    let gradesTable = document.getElementById("gradesTable")
    gradesTable.style.gap = 0

    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString)
    classTitle.innerText = urlParams.get('number') + urlParams.get('letter').toUpperCase() 

    let subjects = await fetchSubjects()
    console.log("subjects", subjects)
    
    // table columns array
    let columns = []
    const columnsCount = 1 + subjects.length
    for (let i = 0; i < columnsCount; i++) {
        columns.push(document.createElement("div"))
        gradesTable.append(columns[i])
    }

    // create header cells
    columns[0].append(await createCell(""))
    for (let i = 1; i < columnsCount; i++) {
        columns[i].append(await createCell(subjects[i-1].name))
    }
    
    
    let students = await fetchStudentsByClass(parseInt(urlParams.get("id")))
    console.log("students", students)

    for (let si = 0; si < students.length; si++) {
        columns[0].append(await createCell(`${students[si].firstName} ${students[si].lastName}`))
        let grades = await fetchGradesByStudent(students[si].id)
        for (let i = 0; i < subjects.length; i++) {
            let curGrade = grades.find((grade) => grade.subjectId == subjects[i].id)
            if (curGrade == undefined) {
                columns[i+1].append(await createCell(""))
            } else {
                columns[i+1].append(await createCell(curGrade.mark))
            }
        }
    }

    let selectorStudent = document.getElementById("selectorStudent")
    students.forEach(student => {
        let newOption = document.createElement("option")
        newOption.innerText = `${student.firstName} ${student.lastName}`
        newOption.value = student.id
        selectorStudent.append(newOption)
    });

    let selectorSubject = document.getElementById("selectorSubject")
    subjects.forEach(subject =>{
        let newOption = document.createElement("option")
        newOption.innerText = `${subject.name}`
        newOption.value = subject.id
        selectorSubject.append(newOption)
    })
    let selectorGrade = document.getElementById("selectorGrade")

    let gradeEditor = document.getElementById("gradeEditor")
    gradeEditor.onsubmit = async (event) => {
        event.preventDefault()
        let newGradeId = null
        let grades = await fetchGradesByStudent(selectorStudent.value)
        let existingGrade = grades.find(grade => grade.subjectId == selectorSubject.value && grade.studentId == selectorStudent.value)
        if (existingGrade != undefined) {
            newGradeId = existingGrade.id
        }
        await handleSubmit(
            newGradeId,
            selectorStudent.value, 
            selectorSubject.value, 
            selectorGrade.value
        )
    }
    classTitle.parentElement.ariaBusy = false
}

(renderPage)()