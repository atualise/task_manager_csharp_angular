import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { TaskService } from '../../../core/services/task.service';
import { TaskFormComponent } from '../task-form/task-form.component';

interface TaskFilters {
  status?: string;
  priority?: string;
}

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.scss']
})
export class TaskListComponent implements OnInit {
  displayedColumns: string[] = ['id', 'title', 'description', 'status', 'priority', 'completionDate', 'actions'];
  dataSource = new MatTableDataSource<any>();
  
  @ViewChild(MatSort) sort!: MatSort;

  filters: TaskFilters = {
    status: '',
    priority: ''
  };

  constructor(
    private taskService: TaskService,
    private dialog: MatDialog
  ) {}

  ngOnInit() {
    this.loadTasks();
  }

  loadTasks() {
    this.taskService.getTasks(this.filters.status, this.filters.priority).subscribe(tasks => {
      this.dataSource.data = tasks;
      this.dataSource.sort = this.sort;
    });
  }

  applyFilter() {
    this.loadTasks();
  }

  openTaskForm(task?: any) {
    const dialogRef = this.dialog.open(TaskFormComponent, {
      data: task
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadTasks();
      }
    });
  }

  deleteTask(id: number) {
    if (confirm('Tem certeza que deseja excluir esta tarefa?')) {
      this.taskService.deleteTask(id).subscribe(() => {
        this.loadTasks();
      });
    }
  }

  exportToExcel() {
    this.taskService.exportToExcel().subscribe((blob: Blob) => {
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
      link.download = 'tarefas.xlsx';
      link.click();
      window.URL.revokeObjectURL(url);
    });
  }
}