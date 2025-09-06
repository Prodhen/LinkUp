pipeline {
    agent any // যে কোনো এভেলেবল এজেন্ট ব্যবহার করো

    environment {
        // Docker Hub Credentials-এর জন্য, যেহেতু এটি 'Username with password' Kind-এর Credential,
        // আমরা এটিকে সরাসরি 'withDockerRegistry' স্টেপের মধ্যে ব্যবহার করব।
        // অথবা, যদি 'sh docker login' ব্যবহার করতে চাই, তাহলে এটিকে ভেঙে Username এবং Password নিতে হবে।
        // এই ক্ষেত্রে, 'withCredentials' ব্লক ব্যবহার করাটা ভালো।

        // local-ssh-key-for-jenkins এর জন্য, 'sshagent' ব্লক ব্যবহার করব।
    }

    stages {
        stage('Checkout Code') {
            steps {
                // আপনার GitHub রিপোজিটরির সঠিক URL এবং Credential ID ব্যবহার করুন
                git branch: 'main', credentialsId: 'github-pat-for-jenkins', url: 'https://github.com/Prodhen/LinkUp.git'
            }
        }

        stage('Client Build & Test') {
            steps {
                dir('client') { // ক্লায়েন্ট ফোল্ডারে যাও
                    sh 'npm install'
                    sh 'npm run build --prod'
                    // Uncomment the line below if you have client tests and want to run them
                    // sh 'npm test'
                }
            }
        }

        stage('API Build & Test') {
            steps {
                dir('API') { // API ফোল্ডারে যাও
                    sh 'dotnet restore'
                    sh 'dotnet build --configuration Release --no-restore'
                    // Uncomment the line below if you have API tests and want to run them
                    // sh 'dotnet test'
                }
            }
        }

        stage('Build & Push Docker Images') {
            steps {
                script {
                    // Docker Hub এ লগইন, বিল্ড এবং পুশ করার জন্য 'docker-hub-credentials-id' ব্যবহার করুন
                    // 'withDockerRegistry' ব্যবহারের জন্য এই Credential ID প্রয়োজন।
                    withDockerRegistry(credentialsId: 'docker-hub-credentials-id', url: 'https://index.docker.io/v1/') {
                        // ক্লায়েন্ট ইমেজ
                        // আপনার Docker Hub ইউজারনেম দিয়ে 'prodhen' এবং ইমেজ নাম দিয়ে 'linkup-client' প্রতিস্থাপন করুন
                        sh "docker build -t prodhen/linkup-client:latest ./client"
                        sh "docker push prodhen/linkup-client:latest"

                        // API ইমেজ
                        // আপনার Docker Hub ইউজারনেম দিয়ে 'prodhen' এবং ইমেজ নাম দিয়ে 'linkup-api' প্রতিস্থাপন করুন
                        sh "docker build -t prodhen/linkup-api:latest ./API"
                        sh "docker push prodhen/linkup-api:latest"
                    }
                }
            }
        }

stage('Deploy on Local PC') {
            steps {
                script {
                    // Define the base path on your Windows host where LinkUp project resides
                    // This path needs to be accessible by the Docker daemon running on your host
                    // It's the path to the directory containing your docker-compose.yml
                    def projectPath = "/cygdrive/d/LinkUp-V1/LinkUp" // Adjust if necessary for your Windows path
                                                                    // (e.g., if you're using Git Bash or Cygwin for Jenkins container launch)

                    // Navigate to the project directory and run docker-compose commands
                    // These commands will execute on the Docker host (your Windows machine)
                    // because we've mounted the Docker socket.
                    sh """
                        # Pull latest images
                        docker pull prodhen/linkup-client:latest
                        docker pull prodhen/linkup-api:latest

                        # Change directory to where docker-compose.yml is located
                        cd ${projectPath}

                        # Stop and remove existing containers, then start new ones
                        docker-compose down
                        docker-compose up -d
                    """
                }
            }
        }
    }
}