import { Component, Input, inject, OnChanges } from '@angular/core';
import { merge, Observable, Subject, tap } from 'rxjs';
import { CommentService, Comment } from '../../Services/comment.service';
import { AsyncPipe, DatePipe } from '@angular/common';
import { LoginPageService } from '../../Services/login-page-service';
import {
  FormsModule,
  ReactiveFormsModule
} from '@angular/forms';

@Component({
  imports: [
    AsyncPipe,
    DatePipe,
    FormsModule,
    ReactiveFormsModule
  ],
  selector: 'app-comment-component',
  styleUrls: ['./comment-component.css'],
  templateUrl: './comment-component.html',
})
export class CommentComponent implements OnChanges {

  private readonly commentService = inject(CommentService);
  private readonly loginService = inject(LoginPageService);

  @Input() bugRefId!: string;

  editingCommentId: string | null = null;
  editedValue = '';

  showNewComment = false;
  newComment = '';

  errorMessage = '';

  private readonly updatedComments$ = new Subject<Comment[]>();
  private latestComments: Comment[] = [];

  comments$!: Observable<Comment[]>;

  currentUser = this.loginService.getCurrentUser()?.username;

  ngOnChanges(): void {
    if (!this.bugRefId) {
      this.errorMessage = 'Bug reference ID is missing';
      return;
    }

    this.comments$ = merge(
      this.commentService.getCommentById(this.bugRefId),
      this.updatedComments$
    ).pipe(
      tap((comments) => {
        this.latestComments = comments;
      })
    );
  }

  editComment(comment: Comment): void {
    if (comment.author !== this.currentUser) {
      return;
    }

    this.editingCommentId = comment.reference_id;
    this.editedValue = comment.comment;
  }

  cancelEdit(): void {
    this.editingCommentId = null;
    this.editedValue = '';
  }

  saveComment(comment: Comment): void {
    const updatedComment = this.editedValue.trim();

    if (!updatedComment) {
      return;
    }

    this.commentService
      .updateComment(comment.reference_id, updatedComment)
      .subscribe({
        next: (updated) => {
          const refreshedComments = this.latestComments.map(existing =>
            existing.reference_id === comment.reference_id
              ? {
                ...existing,
                comment: updatedComment,
                date: updated.date
              }
              : existing
          );

          this.updatedComments$.next(refreshedComments);

          this.editingCommentId = null;
          this.editedValue = '';
        },
        error: (err) => {
          console.error('Error updating comment:', err);
        }
      });
  }

  addComment(): void {
    const comment = this.newComment.trim();

    if (!comment || !this.currentUser) {
      return;
    }

    this.commentService
      .CreateComment(
        this.bugRefId,
        comment,
        this.currentUser
      )
      .subscribe({
        next: (newComment) => {
          this.updatedComments$.next([
            ...this.latestComments,
            newComment
          ]);

          this.newComment = '';
          this.showNewComment = false;
        },
        error: (err) => {
          console.error('Error creating comment:', err);
        }
      });
    window.location.reload();

  }

  cancelNewComment(): void {
    this.newComment = '';
    this.showNewComment = false;
  }
}
