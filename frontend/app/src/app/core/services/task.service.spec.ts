import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TaskService } from './task.service';
import { environment } from '../../environments/environment';
import { Task } from '../models/task.model';

describe('TaskService', () => {
  let service: TaskService;
  let httpMock: HttpTestingController;

  const mockTask: Task = {
    id: 1,
    title: 'Test Task',
    description: 'Test Description',
    status: 'Aberto',
    priority: 'Baixa',
    completionDate: undefined,
    userId: 1,
    createdAt: new Date().toISOString()
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [TaskService]
    });

    service = TestBed.inject(TaskService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get tasks', () => {
    const mockTasks = [mockTask];

    service.getTasks().subscribe(tasks => {
      expect(tasks).toEqual(jasmine.arrayContaining([
        jasmine.objectContaining(mockTask)
      ]));
    });

    const req = httpMock.expectOne(`${environment.taskApi}/task`);
    expect(req.request.method).toBe('GET');
    req.flush(mockTasks);
  });

  it('should create task', () => {
    const newTask = {
      title: 'Test Task',
      description: 'Test Description',
      status: 'Aberto',
      priority: 'Baixa',
      completionDate: undefined
    };

    service.createTask(newTask).subscribe(task => {
      expect(task).toEqual({ ...mockTask, ...newTask });
    });

    const req = httpMock.expectOne(`${environment.taskApi}/task`);
    expect(req.request.method).toBe('POST');
    req.flush({ ...mockTask, ...newTask });
  });

  it('should update task', () => {
    const taskId = 1;
    const updateData = {
      title: 'Updated Task',
      description: 'Updated Description',
      status: 'EmAndamento',
      priority: 'Alta',
      completionDate: new Date()
    };

    service.updateTask(taskId, updateData).subscribe(task => {
      expect(task).toEqual({ ...mockTask, ...updateData });
    });

    const req = httpMock.expectOne(`${environment.taskApi}/task/${taskId}`);
    expect(req.request.method).toBe('PUT');
    req.flush({ ...mockTask, ...updateData });
  });

  it('should delete task', () => {
    const taskId = 1;

    service.deleteTask(taskId).subscribe(response => {
      expect(response).toBeNull();
    });

    const req = httpMock.expectOne(`${environment.taskApi}/task/${taskId}`);
    expect(req.request.method).toBe('DELETE');
    req.flush(null);
  });
}); 