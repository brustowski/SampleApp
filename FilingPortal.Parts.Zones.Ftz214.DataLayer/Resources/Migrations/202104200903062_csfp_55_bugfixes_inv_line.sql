�� 
 - -   a d d   i n v o i c e   l i n e   r e c o r d   - -  
 A L T E R       P R O C E D U R E   [ z o n e s _ f t z 2 1 4 ] . [ s p _ a d d _ i n v o i c e _ l i n e ]   ( @ f i l i n g H e a d e r I d   I N T ,  
 @ p a r e n t I d   I N T ,  
 @ f i l i n g U s e r   N V A R C H A R ( 2 5 5 )   =   N U L L ,  
 @ o p e r a t i o n I d   U N I Q U E I D E N T I F I E R   =   N U L L   O U T P U T )  
 A S  
 B E G I N  
     S E T   N O C O U N T   O N ;  
  
     D E C L A R E   @ t a b l e N a m e   V A R C H A R ( 1 2 8 )   =   ' z o n e s _ f t z 2 1 4 . i n v o i c e _ l i n e ' ;  
     D E C L A R E   @ a l l o w M u l t i p l e   B I T   =   0 ;  
  
     S E T   @ o p e r a t i o n I d   =   C O A L E S C E ( @ o p e r a t i o n I d ,   N E W I D ( ) ) ;  
  
     - -   g e t   s e c t i o n   p r o p e r t y   i s _ a r r a y  
     S E L E C T  
         @ a l l o w M u l t i p l e   =   s e c t i o n . i s _ a r r a y  
     F R O M   z o n e s _ f t z 2 1 4 . f o r m _ s e c t i o n _ c o n f i g u r a t i o n   s e c t i o n  
     W H E R E   s e c t i o n . t a b l e _ n a m e   =   @ t a b l e N a m e  
  
     - -   a d d   i n v o i c e   l i n e   d a t a   a n d   a p p l y   r u l e s  
     I F   @ a l l o w M u l t i p l e   =   1  
         O R   N O T   E X I S T S   ( S E L E C T  
                 1  
             F R O M   z o n e s _ f t z 2 1 4 . i n v o i c e _ l i n e   i n v o i c e _ l i n e  
             W H E R E   i n v o i c e _ l i n e . p a r e n t _ r e c o r d _ i d   =   @ p a r e n t I d )  
     B E G I N  
         I N S E R T   I N T O   z o n e s _ f t z 2 1 4 . i n v o i c e _ l i n e   (  
 	 	   f i l i n g _ h e a d e r _ i d  
 	 	 , p a r e n t _ r e c o r d _ i d  
 	 	 , o p e r a t i o n _ i d  
 	 	 , c r e a t e d _ d a t e  
 	 	 , c r e a t e d _ u s e r  
 	 	 , i n v o i c e _ n o  
 	 	 , z o n e _ s t a t u s  
 	 	 , o r i g i n  
 	 	 , e x p o r t  
 	 	 , t a r i f f  
 	 	 , g o o d s _ d e s c r i p t i o n  
 	 	 , c u s t o m s _ q t y  
 	 	 , i n v o i c e _ q t y  
 	 	 , l i n e _ p r i c e  
 	 	 , m a n u f a c t u r e r _ a d d r e s s  
 	 	 , g r o s s _ w e i g h t  
 	 	 , g r o s s _ w e i g h t _ u n i t  
 	 	 , c h a r g e s  
 	 	 , l o a d i n g _ p o r t  
 	 	 , i n v o i c e _ q t y _ u q  
 	 	 , c o n s i g n e e )  
             S E L E C T  
 	 	   @ f i l i n g H e a d e r I d  
 	 	 , @ p a r e n t I d  
 	 	 , @ o p e r a t i o n I d  
                 , G E T D A T E ( )  
               , @ f i l i n g U s e r  
 	       , p a r s e d _ l i n e . i t e m _ n o  
 	       , p a r s e d _ l i n e . z o n e _ s t a t u s  
 	       , p a r s e d _ l i n e . c o  
 	       , p a r s e d _ d a t a . c e  
 	       , p a r s e d _ l i n e . h t s  
 	       , p a r s e d _ l i n e . d e s c r i p t i o n   - -   g o o d s _ d e s c r i p t i o n  
 	       , p a r s e d _ l i n e . q t y 1  
 	       , p a r s e d _ l i n e . q t y 1  
 	       , p a r s e d _ l i n e . v a l u e  
 	       , p a r s e d _ l i n e . m i d  
 	       , p a r s e d _ l i n e . g r o s s _ w g t  
 	       , p a r s e d _ l i n e . g r o s s _ l b s  
 	       , p a r s e d _ l i n e . c h a r g e s  
 	       , p a r s e d _ d a t a . f o r e i g n _ p o r t  
 	       , d b o . f n _ a p p _ u n i t _ b y _ t a r i f f ( p a r s e d _ l i n e . h t s ,   ' H T S ' )  
 	       , p a r s e d _ l i n e . i m p o r t e r _ i r s n o  
 	   F R O M   z o n e s _ f t z 2 1 4 . f i l i n g _ d e t a i l   d e t a i l  
             J O I N   z o n e s _ f t z 2 1 4 . i n b o u n d   i n b n d  
                 O N   i n b n d . i d   =   d e t a i l . i n b o u n d _ i d  
 	     L E F T   J O I N   z o n e s _ f t z 2 1 4 . i n b o u n d _ p a r s e d _ d a t a   p a r s e d _ d a t a  
 	 	 O N   i n b n d . i d   =   p a r s e d _ d a t a . i d  
  
 	     L E F T   J O I N   z o n e s _ f t z 2 1 4 . i n b o u n d _ p a r s e d _ l i n e   p a r s e d _ l i n e  
 	 	 O N   i n b n d . i d   =   p a r s e d _ l i n e . i d  
  
             L E F T   J O I N   d b o . C l i e n t s   A S   c l i e n t s  
 	 	 O N   i n b n d . a p p l i c a n t _ i d   =   c l i e n t s . i d  
 	 W H E R E   d e t a i l . f i l i n g _ h e a d e r _ i d   =   @ f i l i n g H e a d e r I d  
     E N D ;  
 E N D ;  
 G O  
 A L T E R       P R O C E D U R E   [ z o n e s _ f t z 2 1 4 ] . [ s p _ a d d _ i n v o i c e _ l i n e ]   ( @ f i l i n g H e a d e r I d   I N T ,  
 @ p a r e n t I d   I N T ,  
 @ f i l i n g U s e r   N V A R C H A R ( 2 5 5 )   =   N U L L ,  
 @ o p e r a t i o n I d   U N I Q U E I D E N T I F I E R   =   N U L L   O U T P U T )  
 A S  
 B E G I N  
     S E T   N O C O U N T   O N ;  
  
     D E C L A R E   @ t a b l e N a m e   V A R C H A R ( 1 2 8 )   =   ' z o n e s _ f t z 2 1 4 . i n v o i c e _ l i n e ' ;  
     D E C L A R E   @ a l l o w M u l t i p l e   B I T   =   0 ;  
  
     S E T   @ o p e r a t i o n I d   =   C O A L E S C E ( @ o p e r a t i o n I d ,   N E W I D ( ) ) ;  
  
     - -   g e t   s e c t i o n   p r o p e r t y   i s _ a r r a y  
     S E L E C T  
         @ a l l o w M u l t i p l e   =   s e c t i o n . i s _ a r r a y  
     F R O M   z o n e s _ f t z 2 1 4 . f o r m _ s e c t i o n _ c o n f i g u r a t i o n   s e c t i o n  
     W H E R E   s e c t i o n . t a b l e _ n a m e   =   @ t a b l e N a m e  
  
     - -   a d d   i n v o i c e   l i n e   d a t a   a n d   a p p l y   r u l e s  
     I F   @ a l l o w M u l t i p l e   =   1  
         O R   N O T   E X I S T S   ( S E L E C T  
                 1  
             F R O M   z o n e s _ f t z 2 1 4 . i n v o i c e _ l i n e   i n v o i c e _ l i n e  
             W H E R E   i n v o i c e _ l i n e . p a r e n t _ r e c o r d _ i d   =   @ p a r e n t I d )  
     B E G I N  
         I N S E R T   I N T O   z o n e s _ f t z 2 1 4 . i n v o i c e _ l i n e   (  
 	 	   f i l i n g _ h e a d e r _ i d  
 	 	 , p a r e n t _ r e c o r d _ i d  
 	 	 , o p e r a t i o n _ i d  
 	 	 , c r e a t e d _ d a t e  
 	 	 , c r e a t e d _ u s e r  
 	 	 , i n v o i c e _ n o  
 	 	 , z o n e _ s t a t u s  
 	 	 , o r i g i n  
 	 	 , e x p o r t  
 	 	 , t a r i f f  
 	 	 , g o o d s _ d e s c r i p t i o n  
 	 	 , c u s t o m s _ q t y  
 	 	 , i n v o i c e _ q t y  
 	 	 , l i n e _ p r i c e  
 	 	 , m a n u f a c t u r e r _ a d d r e s s  
 	 	 , g r o s s _ w e i g h t  
 	 	 , g r o s s _ w e i g h t _ u n i t  
 	 	 , c h a r g e s  
 	 	 , l o a d i n g _ p o r t  
 	 	 , i n v o i c e _ q t y _ u q  
 	 	 , c o n s i g n e e  
 	 	 , s p i )  
             S E L E C T  
 	 	   @ f i l i n g H e a d e r I d  
 	 	 , @ p a r e n t I d  
 	 	 , @ o p e r a t i o n I d  
                 , G E T D A T E ( )  
               , @ f i l i n g U s e r  
 	       , p a r s e d _ l i n e . i t e m _ n o  
 	       , p a r s e d _ l i n e . z o n e _ s t a t u s  
 	       , p a r s e d _ l i n e . c o  
 	       , p a r s e d _ d a t a . c e  
 	       , p a r s e d _ l i n e . h t s  
 	       , p a r s e d _ l i n e . d e s c r i p t i o n   - -   g o o d s _ d e s c r i p t i o n  
 	       , p a r s e d _ l i n e . q t y 1  
 	       , p a r s e d _ l i n e . q t y 1  
 	       , p a r s e d _ l i n e . v a l u e  
 	       , p a r s e d _ l i n e . m i d  
 	       , p a r s e d _ l i n e . g r o s s _ w g t  
 	       , ' T '  
 	       , p a r s e d _ l i n e . c h a r g e s  
 	       , p a r s e d _ d a t a . f o r e i g n _ p o r t  
 	       , d b o . f n _ a p p _ u n i t _ b y _ t a r i f f ( p a r s e d _ l i n e . h t s ,   ' H T S ' )  
 	       , c l i e n t s . C l i e n t C o d e  
 	       , ' N / A '  
 	   F R O M   z o n e s _ f t z 2 1 4 . f i l i n g _ d e t a i l   d e t a i l  
             J O I N   z o n e s _ f t z 2 1 4 . i n b o u n d   i n b n d  
                 O N   i n b n d . i d   =   d e t a i l . i n b o u n d _ i d  
 	     L E F T   J O I N   z o n e s _ f t z 2 1 4 . i n b o u n d _ p a r s e d _ d a t a   p a r s e d _ d a t a  
 	 	 O N   i n b n d . i d   =   p a r s e d _ d a t a . i d  
  
 	     L E F T   J O I N   z o n e s _ f t z 2 1 4 . i n b o u n d _ p a r s e d _ l i n e   p a r s e d _ l i n e  
 	 	 O N   i n b n d . i d   =   p a r s e d _ l i n e . i d  
  
             L E F T   J O I N   d b o . C l i e n t s   A S   c l i e n t s  
 	 	 O N   i n b n d . a p p l i c a n t _ i d   =   c l i e n t s . i d  
 	 W H E R E   d e t a i l . f i l i n g _ h e a d e r _ i d   =   @ f i l i n g H e a d e r I d  
     E N D ;  
 E N D ;  
 