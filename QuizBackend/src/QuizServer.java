import java.io.*;
import java.net.*;
import java.util.*;

public class QuizServer {
    private static List<QuizClient> users = new ArrayList<>();
    private static List<String[]> questions = Arrays.asList(
    	    new String[]{"What is 2+2?", "5", "3", "4", "2", "4"},
    	    new String[]{"What is 10-7?", "4", "3", "5", "2", "3"},
    	    new String[]{"What is 3*3?", "8", "6", "12", "9", "9"},
    	    new String[]{"What is 8/2?", "4", "5", "2", "3", "4"},
    	    new String[]{"What is the capital of France?", "Rome", "Paris", "Berlin", "London", "Paris"},
    	    new String[]{"Who wrote 'Romeo and Juliet'?", "Mark Twain", "Jane Austen", "Charles Dickens", "William Shakespeare", "William Shakespeare"},
    	    new String[]{"What is the chemical symbol for water?", "O2", "CO2", "H2SO4", "H2O", "H2O"},
    	    new String[]{"Which planet is known as the Red Planet?", "Saturn", "Mars", "Jupiter", "Venus", "Mars"},
    	    new String[]{"What is the tallest mammal?", "Horse", "Elephant", "Giraffe", "Kangaroo", "Giraffe"},
    	    new String[]{"What is the largest ocean on Earth?", "Arctic Ocean", "Indian Ocean", "Pacific Ocean", "Atlantic Ocean", "Pacific Ocean"},
    	    new String[]{"Who painted the Mona Lisa?", "Vincent van Gogh", "Leonardo da Vinci", "Michelangelo", "Pablo Picasso", "Leonardo da Vinci"},
    	    new String[]{"What is the capital of Japan?", "Kyoto", "Tokyo", "Seoul", "Osaka", "Tokyo"},
    	    new String[]{"What is the chemical symbol for gold?", "Cu", "Au", "Fe", "Ag", "Au"},
    	    new String[]{"Which bird is known for its ability to mimic human speech?", "Duck", "Eagle", "Owl", "Parrot", "Parrot"},   
    	    new String[]{"What is the largest organ inside the human body?", "Heart", "Lungs", "Brain", "Liver", "Liver"},
    	    new String[]{"What is the currency of Brazil?", "Peso", "Yen", "Euro", "Real", "Real"},
    	    new String[]{"What is the capital of Australia?", "Melbourne", "Perth", "Sydney", "Canberra", "Canberra"},
    	    new String[]{"Who developed the theory of relativity?", "Isaac Newton", "Albert Einstein", "Galileo Galilei", "Stephen Hawking", "Albert Einstein"},
    	    new String[]{"Which gas do plants use for photosynthesis?", "Oxygen", "Carbon dioxide", "Nitrogen", "Hydrogen", "Carbon dioxide"},
    	    new String[]{"What is the longest river in the world?", "Yangtze River", "Amazon River", "Nile River", "Mississippi River", "Nile River"},
    	    new String[]{"What is the chemical symbol for sodium?", "Ca", "Na", "K", "Mg", "Na"},
    	    new String[]{"Which animal is known as the 'King of the Jungle'?", "Elephant", "Lion", "Tiger", "Giraffe", "Lion"},
    	    new String[]{"What is the largest planet in our solar system?", "Uranus", "Neptune", "Saturn", "Jupiter", "Jupiter"},
    	    new String[]{"What is the currency of Japan?", "Dollar", "Yen", "Pound", "Euro", "Yen"},
    	    new String[]{"What is the capital of Canada?", "Vancouver", "Ottawa", "Toronto", "Montreal", "Ottawa"},
    	    new String[]{"Who discovered America?", "Amerigo Vespucci", "Vasco da Gama", "Christopher Columbus", "Marco Polo", "Christopher Columbus"},
    	    new String[]{"Who is known as the 'Father of Computers'?", "Bill Gates", "Charles Babbage", "Alan Turing", "Steve Jobs", "Charles Babbage"},
    	    new String[]{"What is the chemical symbol for iron?", "Cu", "Fe", "Ag", "Au", "Fe"},
    	    new String[]{"Which planet is known as the 'Blue Planet'?", "Mars", "Venus", "Earth", "Neptune", "Earth"},
    	    new String[]{"What is the tallest building in the world?", "One World Trade Center", "Abraj Al-Bait Clock Tower", "Burj Khalifa", "Shanghai Tower", "Burj Khalifa"},
    	    new String[]{"What is the currency of India?", "Rupiah", "Rand", "Rupee", "Ringgit", "Rupee"}

    	);

    private static int currentQuestionIndex = 0;
    private static boolean start = false;
	private static int rand =0 ;
	private static List<Integer> preventOldQst = new ArrayList<>();

	public static void main(String[] args) throws IOException {
	    ServerSocket serverSocket = new ServerSocket(5580);

	    boolean maxClientsReached = false;

	    while (true) {
	        if (users.size() < 2) {
	            Socket clientSocket = serverSocket.accept();
	            System.out.println("you are connected.");
	            QuizClient user = new QuizClient(clientSocket);
	            users.add(user);
	            new Thread(user).start();
	        } else {
	            if (!maxClientsReached) {
	                maxClientsReached = true;
	            }
	           // System.out.println("max");
	            Socket clientSocket = serverSocket.accept();

	            PrintWriter pw = new PrintWriter(clientSocket.getOutputStream(), true);
	            // ila wsal Lmax mayzidch ta ykhrj chi wa7d
	            pw.println("Maximum players reached");
	            clientSocket.close();
	        }
	    }
	}

    public static void sendAll(String message, QuizClient sender) {
        for (QuizClient user : users) {
            user.sendMessage(message);
        }
    }

    public static void sendQuesToAll() {
        if (currentQuestionIndex < questions.size()) {
        	do {
                rand = new Random().nextInt(questions.size());
            } while (preventOldQst.contains(rand));

            preventOldQst.add(rand);

            String[] question = questions.get(rand);
            sendAll("QUESTION:" + question[0], null);
            sendAll("CHOICES:" + String.join(",", Arrays.copyOfRange(question, 1, question.length - 1)), null);
            sendAll("ANSWER:" + question[question.length - 1], null);
        }
    }

    public static void startGame() {
        if (users.size() == 2 && !start) {
            start = true;
            for (QuizClient user : users) {
            	
                user.sendMessage("GAME_STARTED PLAYER1:" + users.get(0).playerName + ", PLAYER2:" + users.get(1).playerName);
            }
            sendQuesToAll();
        }
    }
    public static List<QuizClient> getUsers() {
        return users;
    }

    public static List<String[]> getQuestions() {
        return questions;
    }

    public static int getRand() {
        return rand;
    }

    public static int getCurrentQuestionIndex() {
        return currentQuestionIndex;
    }

    public static void incrementCurrentQuestionIndex() {
        currentQuestionIndex++;
    }

}